using System.Windows.Forms;

namespace QuickImageComment.Controls
{
    internal class LabelSetEnabledAppearance : Label
    {
        public void SetEnabledAppearance(bool enabled)
        {
            if (enabled)
            {
                this.ForeColor = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.ForeColorEnabled);
            }
            else
            {
                this.ForeColor = ConfigDefinition.getConfigColor(ConfigDefinition.enumConfigColor.ForeColorNotEnabled);
            }
        }
    }
}
