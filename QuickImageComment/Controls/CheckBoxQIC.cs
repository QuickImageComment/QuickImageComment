using System;
using System.Windows.Forms;

namespace QuickImageComment.Controls
{
    internal class CheckBoxQIC : CheckBox
    {
        public void SetEnabledAppearance(bool enabled)
        {
            if (enabled)
            {
                this.ForeColor = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.ForeColorEnabled);
                this.Tag = null;
            }
            else
            {
                this.ForeColor = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.ForeColorNotEnabled);
                this.Tag = "disabled";
            }
        }
        protected override void OnClick(EventArgs e)
        {
            if ((string)this.Tag == "disabled")
                return;

            base.OnClick(e);
        }

    }
}
