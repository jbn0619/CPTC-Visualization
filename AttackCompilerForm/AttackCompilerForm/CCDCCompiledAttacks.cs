using System;
using System.Collections.Generic;
using System.Text;

namespace AttackCompilerForm
{
    class CCDCCompiledAttacks
    {
        #region Fields

        public List<CCDCAttack> attacks;

        #endregion Fields

        public CCDCCompiledAttacks(List<CCDCAttack> a)
        {
            attacks = a;
        }
    }
}
