using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace

	public partial class CheckListOkGuard {
        public override void Execute()
        {
            // Will throw a GuardException when condition goes wrong
			// Check CheckListOk
			var confirmResult =  MessageBox.Show("Please confirm that condition 'CheckListOk' is valid",
                                    "Confirm Status Change",
                                    MessageBoxButtons.YesNo);
			if (confirmResult != DialogResult.Yes) {
				throw new GuardException(-1, "Condition 'CheckListOk' not valid") ;
			}
            return;
        }
	}

}
// End of Tempate
