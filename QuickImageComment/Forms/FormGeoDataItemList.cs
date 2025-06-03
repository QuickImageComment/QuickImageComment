using System;
using System.Windows.Forms;

namespace QuickImageComment.Forms
{
    public partial class FormGeoDataItemList : Form
    {
        internal GeoDataItem returnGeoDataItem;

        internal FormGeoDataItemList(GeoDataItem returnGeoDataItem)
        {
            InitializeComponent();
        }

        internal void addGeoDataItem(GeoDataItem geoDataItem)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView1);
            row.SetValues(geoDataItem.display_name,
                geoDataItem.country,
                geoDataItem.country_code,
                geoDataItem.state,
                geoDataItem.city,
                geoDataItem.city_district,
                geoDataItem.lat,
                geoDataItem.lon);
            row.Tag = geoDataItem;
            dataGridView1.Rows.Add(row);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            returnGeoDataItem = (GeoDataItem)dataGridView1.SelectedRows[0].Tag;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            returnGeoDataItem = null;
            Close();
        }
    }
}
