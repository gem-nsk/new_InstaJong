using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genField
{
    public class Cell
    {
        private int state = 0;
        private int id = 0;
        private int randomNum = 0;
        public bool changed = false;

        private (int i, int j) coords;
        

        //public Color normCol;
        //public Color partiesCol;

        public Cell() { }
        public Cell(int state) { this.state = state; }
        public Cell(int i, int j) { setCoords(i, j); }

        public int getId() { return id; }
        public int getRandomNum() { return randomNum; }
        public int getState() { return state; }

        public void setId(int id) { this.id = id; }
        public void setRandomNum(int randomNum) { this.randomNum = randomNum; }
        public void setState(int state)
        {
            this.state = state;
        }

        public (int i, int j) getCoords() { return coords; }
        public void setCoords(int i, int j) { this.coords = ValueTuple.Create(i,j);}

    }
}
