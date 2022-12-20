using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delaunay_triangulation_BW.delaunay_triangulation
{
    public class point_list_store
    {
        private Dictionary<int, point_store> all_points;
        private HashSet<int> unique_pointid_list = new HashSet<int>();
      
        //public int points_count { get { return all_points.Count; } }

        public point_list_store()
        {
            // Empty constructor
            this.all_points = new Dictionary<int, point_store>();
        }

        public int add_point(point_store i_pt)
        {
            // Add point
            int point_id = get_unique_point_id();

            this.all_points.Add(point_id,new point_store(point_id, i_pt.pt_coord.x, i_pt.pt_coord.y, i_pt.pt_type));

            return point_id;
        }

        public void remove_point(int pt_id)
        {
            // Remove point
            unique_pointid_list.Add(pt_id);
            this.all_points.Remove(pt_id);
        }

        public void associate_pt_to_edge(int pt1_id,int pt2_id,int edge_id)
        {
            // Add the associated edge
            this.all_points[pt1_id].associated_edge.Add(edge_id);
            this.all_points[pt2_id].associated_edge.Add(edge_id);
        }

        public void dissassociate_pt_from_edge(int pt1_id, int pt2_id, int edge_id)
        {
            // Remove the associated edge
            this.all_points[pt1_id].associated_edge.Remove(edge_id);
            this.all_points[pt2_id].associated_edge.Remove(edge_id);
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


        public List<point_store> get_all_points()
        {
            return this.all_points.Values.ToList();
        }

        public point_store get_point(int pt_id)
        {
            point_store pt;
            if (this.all_points.TryGetValue(pt_id, out pt) == true)
            {
                return pt;
            }
            return null;
        }
    }
}
