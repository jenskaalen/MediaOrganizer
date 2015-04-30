using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaOrganizer.Modules
{
    public interface IModule
    {
        string Name { get; set; }
        void Start();
        void End();
    }
}
