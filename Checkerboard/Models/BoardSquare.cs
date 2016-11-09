using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Checkerboard.Models
{
    public sealed class Board
    {
        private static BoardSquare[,] playingBoard = null;
        private static readonly object padlock = new object();

        private Board()
        {         
        }

        public static BoardSquare[,] Instance
        {
            get
            {
                lock (padlock)
                {
                    if (playingBoard == null)
                    {
                        playingBoard = new BoardSquare[8, 8];
                    }
                    return playingBoard;
                }
            }
        }
    }

    public class BoardSquare
    {
        public bool BoardColor { get; set; }
        public int TeamNumber { get; set; }

        public bool Occupied { get; set; }
        public bool CanMove { get; set; }

        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public bool Selected { get; set; }

        public IPiece Piece;

    }

    public interface IPiece 
    {
        string Name { get;  }
        int Value { get;  }
        void Select( BoardSquare currSquare);
        void Move( BoardSquare currSquare, BoardSquare targetSquare);
        void Deselect(BoardSquare currSquare);
    }

    

    public class Checker : IPiece
    {
        public string Name { get { return "Checker"; } }
        public int Value {  get { return 1; } }        

        BoardSquare[,] boardState = Board.Instance;

        public void Deselect(BoardSquare currSquare)
        {
            boardState[currSquare.XCoord, currSquare.YCoord].Selected = false;
        }

        public void Move(BoardSquare currSquare, BoardSquare targetSquare)
        {
            throw new NotImplementedException();
        }

        public void Select(BoardSquare currSquare)
        {
            //Create Outline on Selected 
            boardState[currSquare.XCoord, currSquare.YCoord].Selected = true;
            // set selected = true; in the BoardSquare Object

            List<BoardSquare> MovementOptions = new List<BoardSquare>();

            //Get Diagonal Objects if They are On the Board
            // up left
            if(currSquare.XCoord > 0 && currSquare.YCoord > 0) {
                BoardSquare UpLeft = boardState[currSquare.XCoord - 1, currSquare.YCoord - 1];
                MovementOptions.Add(UpLeft);
            }
            // up right
            // if(XCoord < 8 && YCoord > 2) {BoardSquare UpRight = Board[XCoord + 1, YCoord - 1];}
            if (currSquare.XCoord < 7 && currSquare.YCoord > 0)
            {
                BoardSquare UpRight = boardState[currSquare.XCoord + 1, currSquare.YCoord - 1];
                MovementOptions.Add(UpRight);
            }
            // down left
            // if(XCoord > 2 && YCoord < 8) {BoardSquare DownLeft = Board[XCoord - 1, YCoord + 1];}
            if (currSquare.XCoord > 0 && currSquare.YCoord < 7)
            {
                BoardSquare DownLeft = boardState[currSquare.XCoord - 1, currSquare.YCoord + 1];
                MovementOptions.Add(DownLeft);
            }
            // down right
            // if(XCoord < 8 && YCoord < 8) {BoardSquare DownRight = Board[XCoord + 1, YCoord + 1];}
            if (currSquare.XCoord <7 && currSquare.YCoord <7)
            {
                BoardSquare DownRight = boardState[currSquare.XCoord + 1, currSquare.YCoord + 1];
                MovementOptions.Add(DownRight);
            }

            // Check for Presence
            foreach(BoardSquare moveOption in MovementOptions)
            {
               if( moveOption.Occupied == false)
                {
                    //Case: Open => Ok to Move
                    boardState[moveOption.XCoord, moveOption.YCoord].CanMove = true;
                //	//Set JS to create outline OnHover
                }
                
               else if (moveOption.Occupied == true)
                {
                    //Case: Occupied + Same Team => Cannot Move
                    if(moveOption.TeamNumber == currSquare.TeamNumber)
                    {
                        boardState[moveOption.XCoord, moveOption.YCoord].CanMove = false;
                    }

                    //Case: Occupied  + Other Team => Can Move and Take
                    if (moveOption.TeamNumber != currSquare.TeamNumber)
                    {
                        boardState[moveOption.XCoord, moveOption.YCoord].CanMove = true;
                    }
                    //	//Set JS to create outline OnHover

                }

            }
            
        }
    }
}