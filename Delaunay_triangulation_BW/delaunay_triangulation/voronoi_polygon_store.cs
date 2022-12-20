using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delaunay_triangulation_BW.delaunay_triangulation
{
    public class voronoi_polygon_store
    {
        public int poly_id { get; private set; }

        public List<point_d> poly_pts = new List<point_d>();

        public point_d mid_pt { get; private set; }

        public voronoi_polygon_store(int i_poly_id, List<point_d> i_poly_pt)
        {
            this.poly_id = i_poly_id;  // assign the id
            this.mid_pt = findCentroid(i_poly_pt);
            this.poly_pts = i_poly_pt.OrderBy(obj => Math.Atan2(obj.x - this.mid_pt.x, obj.y - this.mid_pt.y)).ToList();

        }

        public point_d findCentroid(List<point_d> points)
        {
            double x = 0;
            double y = 0;
            foreach (point_d p in points)
            {
                x += p.x;
                y += p.y;
            }

            double center_x = x / points.Count();
            double center_y = y / points.Count();

            return new point_d(center_x, center_y);
        }

    }
}
