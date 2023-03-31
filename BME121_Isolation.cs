using System;
using static System.Console;

namespace pa1
{
    static class Program
    {
        static int aCurrentRow;
        static int aCurrentColumn;
        static int bCurrentRow;
        static int bCurrentColumn;
        static int[,] board;
        static int aStartRow;
        static int aStartColumn;
        static int bStartRow;
        static int bStartColumn;
        static string[] letters = {"a","b","c","d","e","f","g","h","i","j","k","l",
                "m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
        
        static void Main()
        {
            WriteLine("Instructions: ");
            WriteLine("Goal: Isolate your opponent by removing tiles.");
            WriteLine("Setup:");
            WriteLine("     1. Enter names for player A and B.");
            WriteLine("     2. Enter the dimensions for the playing board(between 4 and 26).");
            WriteLine("     3. Specify starting tiles(any unoccupied tile).");
            WriteLine("     Note: Press enter for default.");            
            WriteLine("On Your turn:");
            WriteLine("     1. Move your pawn to any available surrounding tile(horizontally, vertically or diagonally).");
            WriteLine("     2. Remove any one removeable tile (not starting or occupied).");
            WriteLine("Entering move code:");
            WriteLine("     a = pawn code: row of tile you want pawn to move to");
            WriteLine("     b = pawn code: column of tile you want pawn to move to");
            WriteLine("     c = remove code: row of tile you want to remove");
            WriteLine("     d = remove code: column of tile you want to remove");
            WriteLine();         
            
            //Collecting A's name
            Write("Name of Player A: ");
            string aName = ReadLine( );
            if(aName.Length == 0) aName = "Player A";
            
            //Collecting B's name
            Write("Name of Player B: ");
            string bName = ReadLine( );
            if(bName.Length == 0) bName = "Player B";            
            
            //Collecting number of rows
            int rows;
            Write("How many rows do you want on your game board?(4-26) ");
            string stringRows = ReadLine();
            if(stringRows.Length == 0) rows = 8;
            else rows = int.Parse(stringRows);
            while(rows < 4 || rows > 26)
            {
                Write("Minimum rows = 4, maximum rows = 26, please enter a differnt value: ");
                stringRows = ReadLine();
                if(stringRows.Length == 0) rows = 8;
                else rows = int.Parse(stringRows);
            }
            
            //collecting number of columns
            int columns;
            Write("How many columns do you want in your game board?(4-26) ");
            string stringColumns = ReadLine();
            if(stringColumns.Length == 0) columns = 6;
            else columns = int.Parse(stringColumns);
            while(columns < 4 || columns > 26)
            {
                Write("Minimum columns = 4, maximum columns = 26, please enter a differnt value: ");
                stringColumns = ReadLine();
                if(stringColumns.Length == 0) columns = 6;
                else columns = int.Parse(stringColumns);
            }
            
            
            board = new int[rows,columns]; //1 if A, 2 if B, 3 if start, 4 if available, else if removed
            
            //setting all tiles in board to available
            for(int r = 0; r < rows; r++) 
            {
                for(int c = 0; c < columns; c++)
                {
                    board[r,c] = 4;
                }
            }
            
            DrawBoard();
            
            //A starting position
            Write("Which tile does {0} want to start on?(abcd) ",aName);
            string aStart = ReadLine();
            if(aStart.Length == 0) 
            {
                aStartRow = 0;
                aStartColumn = columns/2;
            }
            else
            {
                aStartRow = Array.IndexOf(letters,(aStart.Substring(0,1)));
                aStartColumn = Array.IndexOf(letters, (aStart.Substring(1,1)));
            }
            aCurrentRow = aStartRow;
            aCurrentColumn = aStartColumn;
            board[aCurrentRow, aCurrentColumn] = 1;
            
            //Collecting B starting position
            Write("Which tile does {0} want to start on?(abcd) ",bName);
            string bStart = ReadLine();
            if(bStart.Length == 0)
            {
                bStartRow = rows-1;
                if(columns%2 == 1) bStartColumn = columns/2;
                else bStartColumn = (columns/2)-1;
            }
            else
            {
                bStartRow = Array.IndexOf(letters,(bStart.Substring(0,1)));
                bStartColumn = Array.IndexOf(letters, (bStart.Substring(1,1)));
                while(isValidRemove(bStartRow, bStartColumn) == false)
                {
                    Write("Invalid starting tile. Please select a different starting tile: ");
                    bStart = ReadLine();
                    if(bStart.Length == 0)
                    {
                        bStartRow = rows-1;
                        if(columns%2 == 1) bStartColumn = columns/2;
                        else bStartColumn = (columns/2)-1;
                    }
                    else
                    {
                        bStartRow = Array.IndexOf(letters,(bStart.Substring(0,1)));
                        bStartColumn = Array.IndexOf(letters, (bStart.Substring(1,1)));
                    }
                }
            }
            bCurrentRow = bStartRow;
            bCurrentColumn = bStartColumn;
            board[bCurrentRow, bCurrentColumn] = 2;
            
            DrawBoard();
            
            //PlayGame --> 
            int aNextRow;
            int aNextColumn;
            int aRemoveRow;
            int aRemoveColumn;
            int bNextRow;
            int bNextColumn;
            int bRemoveRow;
            int bRemoveColumn;
            
            int turn = 0;
            bool gameRunning = true;
            while(gameRunning) //create some evaluation to see if game is done or not
            {
                if(turn%2 == 0)
                {
                    //Player A's turn
                    WriteLine("{0}'s turn.", aName);
                    Write("Enter move code(abcd): ");
                    string aMove = ReadLine();
                    aNextRow = Array.IndexOf(letters,(aMove.Substring(0,1)));
                    aNextColumn = Array.IndexOf(letters,(aMove.Substring(1,1)));
                    aRemoveRow = Array.IndexOf(letters,(aMove.Substring(2,1)));
                    aRemoveColumn = Array.IndexOf(letters,(aMove.Substring(3,1)));
                    
                    //checking validity of move
                    while(isValidMove(aNextRow, aNextColumn, 1) == false)
                    {
                        Write("Invalid pawn postion. Enter a different pawn code(ab): ");
                        aMove = ReadLine();
                        aNextRow = Array.IndexOf(letters,(aMove.Substring(0,1)));
                        aNextColumn = Array.IndexOf(letters,(aMove.Substring(1,1)));
                    }
                    board[aNextRow,aNextColumn] = 1;
                    board[aCurrentRow, aCurrentColumn] = BoardAt(aCurrentRow, aCurrentColumn);
                    
                    //check validity of remove
                    while(isValidRemove(aRemoveRow, aRemoveColumn) == false)
                    {
                        Write("You cannot remove that tile. Enter a different remove code(cd): ");
                        aMove = ReadLine();
                        aRemoveRow = Array.IndexOf(letters,(aMove.Substring(0,1)));
                        aRemoveColumn = Array.IndexOf(letters,(aMove.Substring(1,1)));
                    }
                    board[aRemoveRow,aRemoveColumn] = 5;
                    
                    //setting up for next turn
                    aCurrentRow = aNextRow;
                    aCurrentColumn = aNextColumn;
                    turn ++;
                }
                else
                {
                    //Player B's turn
                    WriteLine("{0}'s turn.", bName);
                    Write("Enter move code(abcd): ");
                    string bMove = ReadLine();
                    bNextRow = Array.IndexOf(letters,(bMove.Substring(0,1)));
                    bNextColumn = Array.IndexOf(letters,(bMove.Substring(1,1)));
                    bRemoveRow = Array.IndexOf(letters,(bMove.Substring(2,1)));
                    bRemoveColumn = Array.IndexOf(letters,(bMove.Substring(3,1)));
                    
                    //checking validity of move
                    while(isValidMove(bNextRow, bNextColumn, 2) == false)
                    {
                        Write("Invalid pawn position. Enter a different pawn code(ab): ");
                        bMove = ReadLine();
                        bNextRow = Array.IndexOf(letters,(bMove.Substring(0,1)));
                        bNextColumn = Array.IndexOf(letters,(bMove.Substring(1,1)));
                    }
                    board[bNextRow,bNextColumn] = 2;
                    board[bCurrentRow, bCurrentColumn] = BoardAt(bCurrentRow, bCurrentColumn);
                    
                    //checking validity of remove
                    while(isValidRemove(bRemoveRow, bRemoveColumn) == false)
                    {
                        Write("You cannot remove that tile. Enter a different remove code(cd): ");
                        bMove = ReadLine();
                        bRemoveRow = Array.IndexOf(letters,(bMove.Substring(0,1)));
                        bRemoveColumn = Array.IndexOf(letters,(bMove.Substring(1,1)));
                    }
                    board[bRemoveRow,bRemoveColumn] = 5;
                    
                    //setting up for next turn
                    bCurrentRow = bNextRow;
                    bCurrentColumn = bNextColumn;
                    turn ++;
                }
                
                DrawBoard();
                
                //checking for vicotry
                bool aHasMovesLeft = HasMovesLeft(1);
                bool bHasMovesLeft = HasMovesLeft(2) ;
                if(aHasMovesLeft == false)
                {
                    WriteLine("{0} does not have any moves left. {1} wins.", aName, bName);
                    gameRunning = false;
                }
                else if(bHasMovesLeft == false)
                {
                    WriteLine("{0} does not have any moves left. {1} wins.", bName, aName);
                    gameRunning = false;
                }
            }
        }
        
        static void DrawBoard()
        {
            const string h  = "\u2500"; // horizontal line
            const string v  = "\u2502"; // vertical line
            const string tl = "\u250c"; // top left corner
            const string tr = "\u2510"; // top right corner
            const string bl = "\u2514"; // bottom left corner
            const string br = "\u2518"; // bottom right corner
            const string vr = "\u251c"; // vertical join from right
            const string vl = "\u2524"; // vertical join from left
            const string hb = "\u252c"; // horizontal join from below
            const string ha = "\u2534"; // horizontal join from above
            const string hv = "\u253c"; // horizontal vertical cross
            const string bb = "\u25a0"; // block
            const string fb = "\u2588"; // full block
            const string lh = "\u258c"; // left half block
            const string rh = "\u2590"; // right half block
            
            WriteLine();
            //Draw top letters
            Write("    ");
            for(int c = 0; c <board.GetLength(1); c++) Write(" {0}  ", letters[c]);
            WriteLine();
            
            //Draw top board boundary
            Write("   ");
            for( int c = 0; c < board.GetLength(1); c++)
            {
                if(c == 0) Write(tl);
                Write("{0}{0}{0}", h);
                if(c == board.GetLength(1)-1) Write("{0}",tr); 
                else Write("{0}",hb);
            }
            WriteLine();
            
            //Draw board rows
            for(int r = 0; r < board.GetLength(0); r++)
            {
                Write(" {0} ", letters[r]); //Writing letter before the rows
                
                //Draw row contents
                for(int c = 0; c < board.GetLength(1); c++)
                {
                    if(c == 0) Write(v);
                    switch(board[r,c])//1 if A, 2 if B, 3 if start, 4 if available, else if removed
                    {
                        case 1:
                            Write("{0}{1}", " A ",v);
                            break;
                        case 2:
                            Write("{0}{1}", " B ",v);
                            break;
                        case 3:
                            Write(" {0} {1}",bb,v);
                            break;
                        case 4:
                            Write("{0}{1}{2}{3}",lh,fb,rh,v);
                            break;
                        default:
                            Write("{0}{1}", "   ",v);
                            break;
                    }
                }
                WriteLine();
                
                //Draw boundary after row
                Write("   ");
                if(r != board.GetLength(0)-1)
                {
                    for(int c = 0; c < board.GetLength(1); c++)
                    {
                        if(c == 0) Write(vr);
                        Write("{0}{0}{0}", h);
                        if(c == board.GetLength(1)-1) Write("{0}",vl); 
                        else Write("{0}",hv);
                    }
                    WriteLine();
                }
                else
                {
                    for(int c = 0; c < board.GetLength(1); c++)
                    {
                        if(c == 0) Write(bl);
                        Write("{0}{0}{0}", h);
                        if(c == board.GetLength(1)-1) Write("{0}",br); 
                        else Write("{0}",ha);
                    }
                    WriteLine();
                }
            }
            WriteLine();
        }
        
        static int BoardAt(int r, int c)
        {
            if ((r == aStartRow) && (c == aStartColumn)) return 3;
            else if((r == bStartRow) && (c == bStartColumn)) return 3;
            else return 4;
        }
        
        static bool isValidMove(int r, int c, int player)
        {
            if(player == 1)
            {
                if(aCurrentRow-1 <= r && r <= aCurrentRow+1)
                {
                    if(aCurrentColumn-1 <= c && c <= aCurrentColumn+1)
                    {
                        if(board[r,c] == 4) return true;
                        else if(board[r,c] == 3) return true;
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else 
            {
                if(bCurrentRow-1 <= r && r <= bCurrentRow+1)
                {
                    if(bCurrentColumn-1 <= c && c <= bCurrentColumn+1)
                    {
                        if(board[r,c] == 4) return true;
                        else if(board[r,c] == 3) return true;
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
        }
        
        static bool isValidRemove(int r, int c)
        {
            if(board[r,c] == 4) return true;
            else return false;
        }
        
        static bool HasMovesLeft (int player)
        {
            for(int r = 0; r < board.GetLength(0); r++)
            {
                for(int c = 0; c < board.GetLength(1); c++)
                {
                    if(isValidMove(r,c,player)) return true;
                }
            }
            return false;
        }
    }
}
