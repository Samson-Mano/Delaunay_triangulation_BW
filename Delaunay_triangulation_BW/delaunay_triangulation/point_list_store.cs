using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delaunay_triangulation_BW.delaunay_triangulation
{
    public class point_list_store
    {
        public HashSet<point_store> all_points { get; private set; }

        public int points_count { get { return all_points.Count; } }

        public point_list_store()
        {
            // Empty constructor
            this.all_points = new HashSet<point_store>();
        }

        public void add_point(point_store i_pt)
        {
            // Add point
            all_points.Add(new point_store(i_pt.pt_id, i_pt.pt_coord.x, i_pt.pt_coord.y, i_pt.pt_type));
        }

        public void remove_point(point_store i_pt)
        {
            // Remove point
            all_points.Remove(i_pt);
        }

        public point_store get_point(int pt_id)
        {
            return all_points.First(p => p.Equals(pt_id));
        }
    }
}
