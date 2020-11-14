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
        public List<BusLine> lines_list { get;  private set; }

        /// <summary>
        /// constract a new "lines" 
        /// </summary>
        /// <param name="lines">save the list as the data base of lines</param>
        public Lines(BusLine[] lines)
        {
            lines_list = new List<BusLine>(lines);
        }

        /// <summary>
        /// add bl to the end of the container
        /// </summary>
        /// <param name="new_line"></param>
        public void add_line(BusLine new_line)
        {
            int count = 0;
            foreach(BusLine item in lines_list)
            {
                if (item.LineNum == new_line.LineNum)
                {
                    if(count  == 1)
                    {
                        throw new LineAlreadyExist("the line already appears twice in the colection");
                    }
                    count++;
                }
            }
            lines_list.Add(new_line);
        }

        /// <summary>
        /// find all the lines which stops at the station with the code "stCode"
        /// </summary>
        /// <param name="stCode"></param>
        /// <returns>a list of all the lines which stops at the station with the code "stCode"</returns>
        /// <exception cref="NotExist"> throw if no line stop at station with the code "stCode"</exception>
        public List<BusLine> which_stops_at(int stCode)
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
                throw new NotExist("no line includes this station");
            }
            return desired_lines;
        }

        /// <summary>
        /// </summary>
        /// <returns>a list of all the lines sorted according to the ride time of the line</returns>
        public List<BusLine> sorted_list()
        {
            lines_list.Sort();
            return lines_list;
        }

        public BusLine this[int lineCode]
        {
            get
            {
                int index = lines_list.FindIndex((BusLine bl) => bl.LineNum == lineCode);
                if(index == -1)
                {
                    throw new NotExist("the line is not exist");
                }
                return lines_list[index];
            }
        }

        public IEnumerator<BusLine> GetEnumerator()
        {
            return lines_list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public override string ToString()
        {
            string output = "";
            foreach (BusLine item in lines_list)
            {
                output += item.ToString() + "\n";
            }
            return output;
        }
    }
}
