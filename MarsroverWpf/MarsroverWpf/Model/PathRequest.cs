using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsroverWpf.Model
{
    public class PathRequest
    {
        public int id;
        public Position start;
        public Position finish;

        PathRequest() { }
        public PathRequest(int id, int startX, int startY, int finishX, int finishY) 
        {
            this.id = id;
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
