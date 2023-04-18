from numpy.fft import fft as numpyFFT
import math

def fft(timeSignal: list) -> list: 
    ''' Computes the 1-dimensional DFT with the FFT
    ## Parameters:
    Input: 
        timeSignal (list): The discrete signal in time domain
    Output: 
        freqSignal (list): The DFT of the signal in freq domain

    ## Raises:
    ValueError:
        if the number of data points in timeSignal is not a power of 2 or is 0
    '''
    #Invalid Cases
    lenx = len(timeSignal)
    if lenx == 0:
        raise ValueError('Invalid number of FFT data points (0) specified.')
    if math.log2(lenx).is_integer() == False:
        raise ValueError('Number of FFT data points ({}) must be a power of 2'.format(lenx))

    #Twiddles
    def getTwiddle(stageNum, butterflyNum):
        angle = 0
        twiddles = []
        if stageNum > 0:
            angle = -1*math.pi/(2**stageNum)
        i = 0
        while i < 2**stageNum:
            twiddles.append(complex(math.cos(i*angle), math.sin(i*angle)))
            i += 1
        return twiddles[butterflyNum%(2**stageNum)]
    
    #Butterfly
    def butterfly(even, odd, twiddle):
        sum = even + odd*twiddle
        diff = even - odd*twiddle
        return sum, diff

    #Helper functions for implementing the fft
    #Note: You start filling in the current stage of the FFT from the earliest unfilled entry
    #      The other entry for the butterfly is determined by which stage number you are in
    #      The indices of the entries you fill in are also the indices of the entries you do the butterfly with 
    def findEarliestOpen(arr, searchStart = 0):
        while searchStart < len(arr):
            if arr[searchStart] == None:
                return searchStart
            searchStart += 1

    #Calculations:
    iterations = math.trunc(math.log2(lenx)) # Number of stages in the FFT
    prevSignal = timeSignal
    curSignal = []

    #Reorder the original list based on the order it should be fed to the butterfly
    for idx in range(lenx):
        binIdx = bin(idx)[2::]
        while len(binIdx) < iterations:
            binIdx = '0'+binIdx
        valIdx = int(binIdx[::-1],2)
        curSignal.append(timeSignal[valIdx])

    #Butterflying
    for stageNum in range(iterations):
        prevSignal = curSignal #PrevSignal holds the output from the previous stage
        curSignal = [None]*lenx
        searchStart = 0
        for butterflyNum in range(math.trunc(lenx/2)):
            #doing a butterfly with a pair of numbers from the previous stage
            earliestOpen = findEarliestOpen(curSignal, searchStart) # Finding the index of the entry to start filling from
            searchStart = earliestOpen + 1 # For the next search, you can start later to speed the search up
            
            twiddle = getTwiddle(stageNum, butterflyNum)
            even, odd = butterfly(prevSignal[earliestOpen], prevSignal[earliestOpen+2**stageNum], twiddle)
            
            curSignal[earliestOpen] = even
            curSignal[earliestOpen+2**stageNum] = odd

    freqSignal = curSignal
    return freqSignal



# TESTING -------------------------------------------------
timeSignals = [
    [7,3,5,2],
    [0,1,2,3,4,5j,6j,7j],
    [0,1,2,3,4,5j,6j,7j,3,1+2j,2+3j,3+5j,4,5j,6j,7j],
    [7,3j]
]

for idx, signal in enumerate(timeSignals):
    print('Test number:', idx, end=' ')

    output = fft(signal)
    for idx, entry in enumerate(output):
        output[idx] = round(entry.real, 2) + round(entry.imag, 2)*1j
    
    trueOutput = list(numpyFFT(signal))
    for idx, entry in enumerate(output):
        trueOutput[idx] = round(entry.real, 2) + round(entry.imag, 2)*1j
    
    eval = 'pass' if output == trueOutput else 'FAIL'
    print(eval)
    if eval == "FAIL":
        print('  ', 'Your Output   :', output)
        print('  ', 'Correct Output:', trueOutput)
