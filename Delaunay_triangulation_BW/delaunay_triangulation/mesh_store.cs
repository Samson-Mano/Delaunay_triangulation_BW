using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Delaunay_triangulation_BW.delaunay_triangulation
{
    public class mesh_store
    {
        List<point_d> boundary_pts = new List<point_d>();
        private int _step_control = 0;
        delaunay_triangulation BW_delaunay;

        public mesh_store()
        {
            // Empty constructor
            BW_delaunay = new delaunay_triangulation();
        }

        public void create_whole_mesh(List<Form1.planar_object_store.point2d> bndr_pts,
                    List<Form1.planar_object_store.point2d> input_points,
                 ref List<Form1.planar_object_store.edge2d> output_edges,
                 ref List<Form1.planar_object_store.face2d> output_triangles)
        {

            _step_control = 0;

            // Convert inpt pts
            List<point_store> input_pt_converted = new List<point_store>();
            List<Form1.planar_object_store.point2d> inpt_pts_ordered = input_points.OrderBy(obj => obj.x).ThenBy(obj => obj.y).ToList();

            // Scale_L value gives enough space to the points
            int scale_L = 100;


            int pt_id = 0;
            foreach (Form1.planar_object_store.point2d pt in inpt_pts_ordered)
            {
                input_pt_converted.Add(new point_store(pt_id, pt.x * scale_L, pt.y * scale_L, 1));
                pt_id++;
            }


            //var watch = new System.Diagnostics.Stopwatch();
            //watch.Start();

            foreach (point_store pt in input_pt_converted)
            {
                if (_step_control == 0)
                {
                    // Create super triangle
                    BW_delaunay.create_super_triangle(input_pt_converted);
                }

                BW_delaunay.Add_single_point(pt.pt_coord);
                _step_control++;

            }

            //watch.Stop();
           //System.Windows.Forms.MessageBox.Show("Elapsed Time for " + _step_control.ToString() + " points = " + ((double)watch.ElapsedMilliseconds) + " ms", "Time taken");

            //string print_time_elapese = "";
            //int pt_count = 1;
            //foreach (double w in time_taken_per_pt)
            //{
            //    print_time_elapese = print_time_elapese + "pt" + pt_count.ToString() + " " + w.ToString() + " ms" + Environment.NewLine;
            //    pt_count++;
            //}

            //Show_error_Dialog("Elapsed Time", print_time_elapese);

            // Remove the super triangle
            // Find all the edges associated with the points 0,1 & 2
            HashSet<int> delete_edge = new HashSet<int>();
            for(int i = 0; i<3;i++)
            {
               delete_edge.UnionWith(BW_delaunay.points_data.get_point(i).associated_edge);
            }

            // Find all the triangles associated with the edges about to be deleted
            HashSet<int> delete_triangle = new HashSet<int>();
            // Delete the edges associated with super triangle
            foreach (int d_edge in delete_edge)
            {
                int l_tri_id = BW_delaunay.edges_data.get_edge(d_edge).left_triangle_id;
                if (l_tri_id != -1)
                {
                    delete_triangle.Add(l_tri_id);
                }

                int r_tri_id = BW_delaunay.edges_data.get_edge(d_edge).right_triangle_id;
                if (r_tri_id != -1)
                {
                    delete_triangle.Add(r_tri_id);
                }

                BW_delaunay.points_data.dissassociate_pt_from_edge(BW_delaunay.edges_data.get_edge(d_edge).start_pt_id,
                    BW_delaunay.edges_data.get_edge(d_edge).end_pt_id, d_edge);
                BW_delaunay.edges_data.remove_edge(d_edge);
            }


            // Delete the super triangle pts
            BW_delaunay.points_data.remove_point(0);
            BW_delaunay.points_data.remove_point(1);
            BW_delaunay.points_data.remove_point(2);

            // Delete the super triangles
            foreach (int d_tri in delete_triangle)
            {
                BW_delaunay.triangles_data.remove_triangle(d_tri);
            }

            // Convert the result to native data
            output_edges = new List<Form1.planar_object_store.edge2d>();
            output_triangles = new List<Form1.planar_object_store.face2d>();

            // Edge data
            foreach (edge_store edge in BW_delaunay.edges_data.get_all_edges())
            {
                point_store pt1 = BW_delaunay.points_data.get_point(edge.start_pt_id);
                point_store pt2 = BW_delaunay.points_data.get_point(edge.end_pt_id);

                Form1.planar_object_store.point2d pt1_o = new Form1.planar_object_store.point2d(pt1.pt_id, pt1.pt_coord.x / (double)scale_L, pt1.pt_coord.y / (double)scale_L);
                Form1.planar_object_store.point2d pt2_o = new Form1.planar_object_store.point2d(pt2.pt_id, pt2.pt_coord.x / (double)scale_L, pt2.pt_coord.y / (double)scale_L);

                Form1.planar_object_store.edge2d edge_o = new Form1.planar_object_store.edge2d(edge.edge_id, pt1_o, pt2_o);

                output_edges.Add(edge_o);
            }


            // Triangles data
            foreach (triangle_store tri in BW_delaunay.triangles_data.get_all_triangles())
            {
                point_store pt1 = BW_delaunay.points_data.get_point(tri.pt1_id);
                point_store pt2 = BW_delaunay.points_data.get_point(tri.pt2_id);
                point_store pt3 = BW_delaunay.points_data.get_point(tri.pt3_id);

                Form1.planar_object_store.point2d pt1_o = new Form1.planar_object_store.point2d(pt1.pt_id, pt1.pt_coord.x / (double)scale_L, pt1.pt_coord.y / (double)scale_L);
                Form1.planar_object_store.point2d pt2_o = new Form1.planar_object_store.point2d(pt2.pt_id, pt2.pt_coord.x / (double)scale_L, pt2.pt_coord.y / (double)scale_L);
                Form1.planar_object_store.point2d pt3_o = new Form1.planar_object_store.point2d(pt3.pt_id, pt3.pt_coord.x / (double)scale_L, pt3.pt_coord.y / (double)scale_L);

                Form1.planar_object_store.face2d face_o = new Form1.planar_object_store.face2d(tri.tri_id, pt1_o, pt2_o, pt3_o);

                output_triangles.Add(face_o);
            }
        }

        public static void Show_error_Dialog(string title, string text)
        {
            var form = new Form()
            {
                Text = title,
                Size = new Size(800, 600)
            };

            form.Controls.Add(new TextBox()
            {
                Font = new Font("Segoe UI", 12),
                Text = text,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill
            });

            form.ShowDialog();
            form.Controls.OfType<TextBox>().First().Dispose();
            form.Dispose();
        }

        public void smooth_mesh()
        {


        }

        public void make_convex_hull()
        {
            //if(BW_delaunay.edges_data.get_all_edges().Count == 0)
            //{
            //    // meshing is not done so exit
            //    return;
            //}

            //HashSet<int> delete_edge = new HashSet<int>();

            //// Convex Hull
            //foreach(point_store pts in BW_delaunay.points_data.get_all_points())
            //{
            //    // Get all the associated edges
            //    List<int> assoc_edges = pts.associated_edge.ToList();

            //    // Get all the other pt coords
            //    List<point_d> surround_pts = new List<point_d>();

            //    // Cycle through all associated edges
            //    foreach(int a_ed in assoc_edges)
            //    {
            //        int other_pt_id = BW_delaunay.edges_data.get_edge(a_ed).other_point_id(pts.pt_id);
            //        // Add the other pt data to list
            //        surround_pts.Add(BW_delaunay.points_data.get_point(other_pt_id).pt_coord);
            //    }

            //    // Sort the surround points counter-clockwise
            //    edge_angle_comparer_vertical ed_comparer = new edge_angle_comparer_vertical(pts.pt_coord);
            //    surround_pts = surround_pts.OrderBy(a => a,ed_comparer).ToList();

            //    // Check whether the given pt is inside the surround points ie., they form a closed loop
            //    if(is_point_in_boundary(surround_pts, pts.pt_coord) == true)
            //    {
            //       // Point is inside the surround points
            //        delete_edge.UnionWith(assoc_edges.ToHashSet());
            //    }
            //}


            //// Find all the triangles associated with the edges about to be deleted
            //HashSet<int> delete_triangle = new HashSet<int>();
            //// Delete the edges associated with super triangle
            //foreach (int d_edge in delete_edge)
            //{
            //    int l_tri_id = BW_delaunay.edges_data.get_edge(d_edge).left_triangle_id;
            //    if (l_tri_id != -1)
            //    {
            //        delete_triangle.Add(l_tri_id);
            //    }

            //    int r_tri_id = BW_delaunay.edges_data.get_edge(d_edge).right_triangle_id;
            //    if (r_tri_id != -1)
            //    {
            //        delete_triangle.Add(r_tri_id);
            //    }

            //    BW_delaunay.edges_data.remove_edge(d_edge);
            //}

            //// Delete the super triangles
            //foreach (int d_tri in delete_triangle)
            //{
            //    BW_delaunay.triangles_data.remove_triangle(d_tri);
            //}


        }

        public class edge_angle_comparer_vertical : IComparer<point_d>
        {
            //   pt
            //   |\
            //   | \
            //   |  \
            //   |   \
            //   V    V

            private point_d pt;

            public edge_angle_comparer_vertical(point_d i_pt)
            {
                this.pt = i_pt;
            }

            public int Compare(point_d pt1, point_d pt2)
            {
                // if return is less than 0 (then e1 is less than e2)
                // if return equals 0 (then e1 is equal to e2)
                // if return is greater than 0 (then e1 is greater than e2)
                double angle_e1, angle_e2;

                point_d y_pt = new point_d(this.pt.x, this.pt.y - 10000);
                angle_e1 = angle_between(y_pt, pt1);
                 angle_e2 = angle_between(y_pt, pt2);

                // A signed integer that indicates the relative values of x and y:
                //  -If less than 0, x is less than y.
                //  - If 0, x equals y.
                //  - If greater than 0, x is greater than y.
                if (angle_e1 < angle_e2)
                {
                    return -1;
                }
                else if (angle_e1 > angle_e2)
                {
                    return +1;
                }
                else
                {
                    return 0; // Zero is not a case (never). If this line executes something went wrong!!
                }
            }

            private double angle_between(point_d pt1, point_d pt2)
            {
                double v1_x, v1_y;
                double v2_x, v2_y;
                double normalzie;
                // vector with edge
                v1_x = pt1.x - pt.x;
                v1_y = pt1.y - pt.y;
                normalzie = Math.Sqrt(Math.Pow(v1_x, 2) + Math.Pow(v1_y, 2));

                v1_x = v1_x / normalzie;
                v1_y = v1_y / normalzie;

                // vector the edge
                v2_x = pt2.x - pt.x;
                v2_y = pt2.y - pt.y;
                normalzie = Math.Sqrt(Math.Pow(v2_x, 2) + Math.Pow(v2_y, 2));

                v2_x = v2_x / normalzie;
                v2_y = v2_y / normalzie;

                // sin and cos of the two vectors
                double sin = (v1_x * v2_y) - (v2_x * v1_y);
                double cos = (v1_x * v2_x) + (v1_y * v2_y);

                double angle = (Math.Atan2(sin, cos) / Math.PI) * 180f;
                if (angle <= 0) // there is no zero degree (zero degree = 360 degree) to avoid the vertical line mismatch
                    angle += 360f;

                return angle;
            }


        }

        private bool is_point_in_boundary(List<point_d> bndry_pts, point_d i_pt)
        {
            // Get the angle between input_pt and the first & last boundary pt
          
            double t_angle = point_d.GetAngle(bndry_pts.Last(), i_pt, bndry_pts.First());

            // Add the angle  to the inpt_pt and other boundary pts
            for (int i = 0; i < bndry_pts.Count - 1; i++)
            {
                t_angle += point_d.GetAngle(bndry_pts[i], i_pt, bndry_pts[i + 1]);
            }
            return (Math.Abs(t_angle) > 1);
        }



        public void create_mesh_step_by_step(List<Form1.planar_object_store.point2d> bndr_pts,
                 List<Form1.planar_object_store.point2d> input_points,
                 ref List<Form1.planar_object_store.edge2d> output_edges,
                 ref List<Form1.planar_object_store.face2d> output_triangles,
                 int step_control)
        {

            // Convert inpt pts
            List<point_store> input_pt_converted = new List<point_store>();
            List<Form1.planar_object_store.point2d> inpt_pts_ordered = input_points.OrderBy(obj => obj.x).ThenBy(obj => obj.y).ToList();

            int pt_id = 0;
            foreach (Form1.planar_object_store.point2d pt in inpt_pts_ordered)
            {
                input_pt_converted.Add(new point_store(pt_id, pt.x, pt.y, 1));
                pt_id++;
            }



            if (step_control == 0)
            {
                // Create super triangle
                BW_delaunay.create_super_triangle(input_pt_converted);
            }

            // Add to the mesh point by point
            point_store pt_i = input_pt_converted[step_control];
            BW_delaunay.Add_single_point(pt_i.pt_coord);



            // Convert the result to native data
            output_edges = new List<Form1.planar_object_store.edge2d>();
            output_triangles = new List<Form1.planar_object_store.face2d>();

            // Edge data
            foreach (edge_store edge in BW_delaunay.edges_data.get_all_edges())
            {
                point_store pt1 = BW_delaunay.points_data.get_point(edge.start_pt_id);
                point_store pt2 = BW_delaunay.points_data.get_point(edge.end_pt_id);

                Form1.planar_object_store.point2d pt1_o = new Form1.planar_object_store.point2d(pt1.pt_id, pt1.pt_coord.x, pt1.pt_coord.y);
                Form1.planar_object_store.point2d pt2_o = new Form1.planar_object_store.point2d(pt2.pt_id, pt2.pt_coord.x, pt2.pt_coord.y);

                Form1.planar_object_store.edge2d edge_o = new Form1.planar_object_store.edge2d(edge.edge_id, pt1_o, pt2_o);

                output_edges.Add(edge_o);
            }


            // Triangles data
            foreach (triangle_store tri in BW_delaunay.triangles_data.get_all_triangles())
            {
                point_store pt1 = BW_delaunay.points_data.get_point(tri.pt1_id);
                point_store pt2 = BW_delaunay.points_data.get_point(tri.pt2_id);
                point_store pt3 = BW_delaunay.points_data.get_point(tri.pt3_id);

                Form1.planar_object_store.point2d pt1_o = new Form1.planar_object_store.point2d(pt1.pt_id, pt1.pt_coord.x, pt1.pt_coord.y);
                Form1.planar_object_store.point2d pt2_o = new Form1.planar_object_store.point2d(pt2.pt_id, pt2.pt_coord.x, pt2.pt_coord.y);
                Form1.planar_object_store.point2d pt3_o = new Form1.planar_object_store.point2d(pt3.pt_id, pt3.pt_coord.x, pt3.pt_coord.y);

                Form1.planar_object_store.face2d face_o = new Form1.planar_object_store.face2d(tri.tri_id, pt1_o, pt2_o, pt3_o);

                output_triangles.Add(face_o);
            }




            // Add step
            this._step_control++;

        }

    }
}
