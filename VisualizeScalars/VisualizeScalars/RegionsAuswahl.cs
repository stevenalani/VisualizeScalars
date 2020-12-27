using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using VisualizeScalars.DataQuery;

namespace VisualizeScalars
{
    public partial class RegionSelectionForm : Form
    {
        public delegate void GetImageDelegate(long time);

        public GetImageDelegate getImageDelegate;
#if DEBUG
        private Bitmap Image;
#endif
        public GMapOverlay OverlayMarkers = new GMapOverlay("Markers");
        public Bitmap TargetBitmap;

        public RegionSelectionForm(DataFileQuerent querent)
        {
            DataQuerernt = querent;
            InitializeComponent();
        }

        private DataFileQuerent DataQuerernt { get; }

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
            gMapControl1.Zoom = 15;
            gMapControl1.Position = new PointLatLng(48.75 + (48.8 - 48.75) / 2, 9.1566 + (9.2 - 9.1566) / 2);
            gMapControl1.MapProvider = GMapProviders.BingMap;
            gMapControl1.Overlays.Add(OverlayMarkers);
            gMapControl1.OnMapClick += GMapControl1_OnMapClick;
            gMapControl1.OnMarkerClick += (item, args) =>
            {
                if (args.Button == MouseButtons.Right)
                    OverlayMarkers.Markers.Remove(item);
            };
            getImageDelegate = GetBitmapOnTileLoad;
        }

        private void GMapControl1_OnMapClick(PointLatLng pointClick, MouseEventArgs e)
        {
            if (OverlayMarkers.Markers.Count > 1) return;
            if (OverlayMarkers.Markers.Count == 0 || OverlayMarkers.Markers.Any(x => (string) x.Tag == "marker2"))
                OverlayMarkers.Markers.Add(new GMarkerGoogle(pointClick, GMarkerGoogleType.red_dot) {Tag = "marker1"});
            else if (OverlayMarkers.Markers.Count == 1)
                OverlayMarkers.Markers.Add(new GMarkerGoogle(pointClick, GMarkerGoogleType.green_dot)
                    {Tag = "marker2"});
        }

        public DataGrid<GridCell> GetDataGrid()
        {
            if (OverlayMarkers.Markers.Count == 0)
            {
                MessageBox.Show("Start oder Endemarker fehlt");
                return null;
            }
            
            var lats = OverlayMarkers.Markers.Select(x => x.Position.Lat);
            var lngs = OverlayMarkers.Markers.Select(x => x.Position.Lng);
            var dataSet = DataQuerernt.GetDataForArea(lats.Min(), lngs.Min(), lats.Max(), lngs.Max());
            var gridsize = DataQuerernt.GetGridUnitStep();
            return new DataGrid<GridCell>(dataSet, gridsize, lats.Min(), lngs.Min());
        }


        private void GetBitmapOnTileLoad(long elapsedMilliseconds)
        {
            //Task.Delay(3000);
            if (InvokeRequired)
            {
                Invoke(getImageDelegate, (object) elapsedMilliseconds);
            }
            else
            {
                gMapControl1.ZoomAndCenterMarkers(OverlayMarkers.Id);
                gMapControl1.ReloadMap();
                Task.Delay(200);
                var posY = OverlayMarkers.Markers.Select(x => gMapControl1.Height / 2 + x.LocalPosition.Y - x.Offset.Y);
                var posX = OverlayMarkers.Markers.Select(x => gMapControl1.Width / 2 + x.LocalPosition.X - x.Offset.X);
                var width = posX.Max() - posX.Min();
                var height = posY.Max() - posY.Min();
                var image = (Bitmap) gMapControl1.ToImage();
                TargetBitmap = new Bitmap(width, height);
                var cropRectangle = new Rectangle(posX.Min(), posY.Min(), width, height);
                using (var g = Graphics.FromImage(TargetBitmap))
                {
                    g.DrawImage(image, new Rectangle(0, 0, TargetBitmap.Width, TargetBitmap.Height),
                        cropRectangle,
                        GraphicsUnit.Pixel);
                }

#if DEBUG
                image.Save("D:\\inputimage.png");
                TargetBitmap.Save("D:\\outputimage.png");
#endif
            }
        }

        public void LoadImageData()
        {
            gMapControl1.OnTileLoadStart += TileLoadStart;
        }

        private void TileLoadStart()
        {
            gMapControl1.OnTileLoadComplete += GetBitmapOnTileLoad;
            gMapControl1.OnTileLoadStart -= TileLoadStart;
        }
    }
}