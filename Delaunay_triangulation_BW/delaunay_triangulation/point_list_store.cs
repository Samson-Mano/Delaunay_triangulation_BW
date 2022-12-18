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
        private HashSet<int> unique_pointid_list = new HashSet<int>();

        public int points_count { get { return all_points.Count; } }

        public point_list_store()
        {
            // Empty constructor
            this.all_points = new HashSet<point_store>();
        }

        public void add_point(point_store i_pt)
        {
            // Add point
            int point_id = get_unique_point_id();

            all_points.Add(new point_store(point_id, i_pt.pt_coord.x, i_pt.pt_coord.y, i_pt.pt_type));
        }

        public void remove_point(point_store i_pt)
        {
            // Remove point
            unique_pointid_list.Add(i_pt.pt_id);
            all_points.Remove(i_pt);
        }

        private int get_unique_point_id()
        {
            int point_id;
            // get an unique edge id
            if (unique_pointid_list.Count != 0)
            {
                point_id = unique_pointid_list.First(); // retrive the edge id from the list which stores the id of deleted edges
                unique_pointid_list.Remove(point_id); // remove that id from the unique edge id list
            }
            else
            {
                point_id = this.all_points.Count;
            }
            return point_id;
        }

        public point_store get_point(int pt_id)
        {
            return all_points.First(p => p.Equals(pt_id));
        }
    }
}
