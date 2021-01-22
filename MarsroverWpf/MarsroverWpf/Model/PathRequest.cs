using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsroverWpf.Model
{
    public class PathRequest
    {
        public Position start;
        public Position finish;
        public FieldToSave map;

        PathRequest() { }
        public PathRequest(FieldToSave map, int startX, int startY, int finishX, int finishY) 
        {
            this.map = map;
            start = new Position(startX, startY);
            finish = new Position(finishX, finishY);
        }
    }

    public class Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
