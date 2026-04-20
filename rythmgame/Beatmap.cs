using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rythmgame
{
    public class Beatmap
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public int BPM { get; set; }
        public List<Beatnote> notes { get; set; }

    }
}
