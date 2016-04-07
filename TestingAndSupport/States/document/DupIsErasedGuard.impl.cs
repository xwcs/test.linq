using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using xwcs.core.statemachine;

namespace WF2.States.document { // StateMachineNamespace

	public partial class DupIsErasedGuard {
        public override void Execute()
        {
            // Will throw a GuardException when condition goes wrong
			// Check DupIsErased
			var confirmResult =  MessageBox.Show("Please confirm that condition 'DupIsErased' is valid",
                                    "Confirm Status Change",
                                    MessageBoxButtons.YesNo);
			if (confirmResult != DialogResult.Yes) {
				throw new GuardException(-1, "Condition 'DupIsErased' not valid") ;
			}
            return;
        }
	}

}
// End of Tempate
