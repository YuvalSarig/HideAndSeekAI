using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNS
{
    // delegate
    public delegate void DelgtDraw();
    public delegate void DelgtUpdate();
    // enum
    enum NeuronTypes { INPUT, HIDDEN, OUTPUT }
    enum GroundTypes { Obstacle, Way };
}
