using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IPlayerAction
{
    void UpdatePosition(bool onGround, float x, float y, float z);
}
