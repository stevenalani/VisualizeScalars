using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using SixLabors.ImageSharp.Formats.Png;
using VisualizeScalars.DataQuery;

namespace VisualizeScalars
{
    public partial class RegionSelectionForm : Form
    {
        public GMapOverlay OverlayMarkers = new GMapOverlay("Markers");

        public RegionSelectionForm(DataFileQuerent querent)
        {
            this.DataQuerernt = querent;
            InitializeComponent();
        }

        private DataFileQuerent DataQuerernt { get; set; }

        private void Regions__Auswahl_Load(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            gMapControl1.RoutesEnabled = true;
            gMapControl1.PolygonsEnabled = true;
            gMapControl1.MarkersEnabled = true;
            gMapControl1.NegativeMode = false;
            gMapControl1.RetryLoadTile = 0;
            gMapControl1.ShowTileGridLines = false;
            gMapControl1.AllowDrop = true;
            gMapControl1.IgnoreMarkerOnMouseWheel = true;
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.DisableFocusOnMouseEnter = true;
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 9;
            gMapControl1.Position = new PointLatLng(48.75 + (48.8 - 48.75) / 2, 9.1566 + (9.2 - 9.1566) / 2);
            gMapControl1.MapProvider = GMapProviders.BingMap;
            gMapControl1.Overlays.Add(OverlayMarkers);
            gMapControl1.OnMapClick += GMapControl1_OnMapClick;
            gMapControl1.OnMarkerClick += (item, args) =>
            {
                if (args.Button == MouseButtons.Right)
                    OverlayMarkers.Markers.Remove(item);

            };
        }
        private void GMapControl1_OnMapClick(PointLatLng pointClick, MouseEventArgs e)
        {
            if (OverlayMarkers.Markers.Count > 1)
            {
                return;
            }
            if (OverlayMarkers.Markers.Count == 0 || OverlayMarkers.Markers.Any(x => (string)x.Tag == "marker2"))
                OverlayMarkers.Markers.Add(new GMarkerGoogle(pointClick, GMarkerGoogleType.red_dot) { Tag = "marker1" });
            else if (OverlayMarkers.Markers.Count == 1)
                OverlayMarkers.Markers.Add(new GMarkerGoogle(pointClick, GMarkerGoogleType.green_dot) { Tag = "marker2" });
        }

        public DataGrid<GridCell> GetDataGrid() 
        {
            if (OverlayMarkers.Markers.Count < 0) MessageBox.Show("Start oder Endemarker fehlt");
            var lats = OverlayMarkers.Markers.Select(x => x.Position.Lat);
            var lngs = OverlayMarkers.Markers.Select(x => x.Position.Lng);
            var dataSet = DataQuerernt.GetDataForArea(lats.Min(), lngs.Min(), lats.Max(), lngs.Max());
            var gridsize = DataQuerernt.GetGridUnitStep();
            return new DataGrid<GridCell>(dataSet, gridsize, lats.Min(), lngs.Min());
            
        }
        public byte[] GetImageData()
        {
            var lats = OverlayMarkers.Markers.Select(x => x.LocalPosition.Y - x.Offset.Y);
            var lngs = OverlayMarkers.Markers.Select(x => x.LocalPosition.X - x.Offset.X);
            int width = lngs.Max() - lngs.Min();
            int height = lats.Max() - lats.Min();
            var image = (Bitmap) gMapControl1.ToImage();
            var target = new Bitmap(width,height);
            Rectangle cropRectangle = new Rectangle(gMapControl1.Width/2 + lngs.Min(), gMapControl1.Height/2 + lats.Min(),width,height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(image, new Rectangle(0, 0, target.Width, target.Height),
                    cropRectangle,
                    GraphicsUnit.Pixel);
            }
            target.Save("D:\\outputimage.png");
            return null;

        }
    }
}
