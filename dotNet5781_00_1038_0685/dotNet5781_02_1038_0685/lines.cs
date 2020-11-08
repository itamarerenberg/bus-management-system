using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_1038_0685
{
    class Lines : IEnumerable<BusLine>
    {
        private List<BusLine> lines_list { get; set; }

        public Lines(BusLine[] lines)
        {
            lines_list = new List<BusLine>(lines);
        }

        public void add_line(BusLine bl)
        {
            int count = 0;
            foreach(BusLine item in lines_list)
            {
                if (item.LineNum == bl.LineNum)
                {
                    if(count  == 1)
                    {
                        throw new LineAlreadyExist("the line allready appears twice in the colection");
                    }
                    count++;
                }
            }
            lines_list.Add(bl);
        }

        public List<BusLine> which_stops_by(int stCode)
        {
            List<BusLine> desired_lines = new List<BusLine>();
            foreach(BusLine item in lines_list)
            {
                if(item.Station_in_the_line(stCode))
                {
                    desired_lines.Add(item);
                }
            }
            if(desired_lines.Count == 0)
            {
                throw new DontExist("no line includes this station");
            }
            return desired_lines;
        }

        public List<BusLine> sorted_list()
        {
            lines_list.Sort();
            return lines_list;
        }

        public BusLine this[int lineCode]
        {
            get => lines_list[lines_list.FindIndex((BusLine bl) => bl.LineNum == lineCode)]; 
            set => lines_list[lines_list.FindIndex((BusLine bl) => bl.LineNum == lineCode)] = value;
        }

        public IEnumerator<BusLine> GetEnumerator()
        {
            return lines_list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}
