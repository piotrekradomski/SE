﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareEngineering_project
{



    public abstract class Player
    {
        protected int position_x, position_y;
        protected Bitmap bmp;
        protected Piece carrying = null;

        public Player()
        {
            bmp = Properties.Resources.test;
        }

        // setters and getters 
        public int getPosX()
        {
            return position_x;
        }
        public int getPosY()
        {
            return position_y;
        }
        public void setPosX(int x) {
            position_x = x;
        }
        public void setPosY(int y) {
            position_y = y;
        }
        public Piece getCarrying() {
            return carrying;
        }

        public abstract Bitmap getBitmap();

        public bool canMove(int x, int y)
        {
            // if pos is out of board or player bounds, or occupied
            if (withinBoardBounds(x, y) == false)
            {
                return false;
            }
            if (withinPlayerBounds(y) == false)
            {
                return false;
            }
            if (!MyGlobals.boardView1.isFreeOfPlayer(x, y))
            {
                return false;
            }

            // if checks passed, it can move
            return true;
        }

        // movement also updates coords of pieces if they are being carried by the player that is moving
        public bool MoveUp()
        {

            // check if it can move
            if (!canMove(getPosX(), getPosY() - 1))
            {
                return false;
            }

            // if it can move, set new position
            this.setPosY(getPosY() - 1);

            // if the player carries a piece, update piece location as well
            if (this.carrying != null) {
                //new coord Y of piece will be equal to new coord Y of player
                carrying.setPosY(this.getPosY()); 
            }

            // return success
            return true;
        }

        public bool MoveDown()
        {

            // check if it can move
            if (!canMove(getPosX(), getPosY() + 1))
            {
                return false;
            }

            // if it can move, set new position
            this.setPosY(getPosY() + 1);

            // if the player carries a piece, update piece location as well
            if (this.carrying != null)
            {
                //new coord Y of piece will be equal to new coord Y of player
                carrying.setPosY(this.getPosY());
            }


            // return success
            return true;
        }

        public bool MoveLeft()
        {
            // check if it can move
            if (!canMove(getPosX() - 1, getPosY()))
            {
                return false;
            }

            // if it can move, set new position
            this.setPosX(getPosX() - 1);

            // if the player carries a piece, update piece location as well
            if (this.carrying != null)
            {
                //new coord X of piece will be equal to new coord X of player
                carrying.setPosX(this.getPosX());
            }

            // return success
            return true;
        }

        public bool MoveRight()
        {
            // check if it can move
            if (!canMove(getPosX() + 1, getPosY()))
            {
                return false;
            }

            // if it can move, set new position
            this.setPosX(getPosX() + 1);

            // if the player carries a piece, update piece location as well
            if (this.carrying != null)
            {
                //new coord X of piece will be equal to new coord Y of player
                carrying.setPosX(this.getPosX());
            }

            // return success
            return true;
        }
        
        // check if position is out of the board bounds
        public bool withinBoardBounds(int x, int y) {
            // if coord x is out of bounds
            if (x < 0 || x > MyGlobals.Width -1) {
                return false;
            }
            // if coord y is out of bounds
            if (y < 0 || y > MyGlobals.Height - 1)
            {
                return false;
            }
            // if all inside bounds, return success
            return true;
        }

        public abstract bool withinPlayerBounds(int y);

        public bool canPlacePlayer(int x, int y)
        {

            // if a player or piece with same coords is already there, return failure
            foreach (Player item in MyGlobals.redPlayers)
            {
                if (x == item.getPosX() && y == item.getPosY())
                {
                    return false;
                }
            }
            foreach (Player item in MyGlobals.bluePlayers)
            {
                if (x == item.getPosX() && y == item.getPosY())
                {
                    return false;
                }
            }
            foreach (Piece item in MyGlobals.pieces)
            {
                if (x == item.getPosX() && y == item.getPosY())
                {
                    return false;
                }
            }


            // if there is no duplicate return success
            return true;
        }

        // try to pick piece from own location
        public bool pickPiece() {
            Piece pi = Piece.findPiece(position_x, position_y);

            // if the piece does not exist, return failure
            if (pi == null) {
                return false;
            }

            // if the piece exists but it is carried by someone else, return failure
            if (pi.getOwner() != null) {
                return false;
            }

            //if the piece exists and is not carried by another player, pick it
            this.carrying = pi;
            pi.setOwner(this);
            return true;
        }
    }

    public class BluePlayer : Player
    {
        public BluePlayer() : base()
        {
            // try to place player in tasks area
            do
            {
                position_x = MyGlobals.rnd.Next(0, MyGlobals.Width);
                position_y = MyGlobals.rnd.Next(MyGlobals.smallHeight, MyGlobals.Height - MyGlobals.smallHeight - 1);

            } while (!canPlacePlayer(position_x, position_y));

            MyGlobals.bluePlayers.Add(this);
            //   bmp = Properties.Resources.test;
        }

        public override Bitmap getBitmap()
        {
            return bmp;
        }

        // check if blue plyer does not step into red player's goal area
        public override bool withinPlayerBounds(int y) {
            if (y < MyGlobals.smallHeight)
            {
                return false;
            }
            return true;
        }

    }

    public class RedPlayer : Player
    {
       public RedPlayer() : base()
        {
            // try to place player in tasks area
            do {
                position_x = MyGlobals.rnd.Next(0, MyGlobals.Width);
                position_y = MyGlobals.rnd.Next(MyGlobals.smallHeight, MyGlobals.Height - MyGlobals.smallHeight - 1);
    
            } while(!canPlacePlayer(position_x, position_y));

            MyGlobals.redPlayers.Add(this);

        }
        public override Bitmap getBitmap()
        {
            return bmp;
        }

        // check if red plyer does not step into blue player's goal area
        public override bool withinPlayerBounds(int y) {
            if (y > (MyGlobals.Height - MyGlobals.smallHeight-1) ) {
                return false;
            }
            return true;
        }

        





    }

}
