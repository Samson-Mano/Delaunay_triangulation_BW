using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delaunay_triangulation_BW.delaunay_triangulation
{
    public class mesh_store
    {
        List<point_d> boundary_pts = new List<point_d>();
        private int _step_control = 0;
        delaunay_triangulation BW_delaunay = new delaunay_triangulation();

        public mesh_store()
        {

        }

        public void clear_mesh()
        {

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


            int pt_id = 0;
            foreach (Form1.planar_object_store.point2d pt in inpt_pts_ordered)
            {
                input_pt_converted.Add(new point_store(pt_id, pt.x, pt.y, 1));
                pt_id++;
            }

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

            // Convert the result to native data
            output_edges = new List<Form1.planar_object_store.edge2d>();
            output_triangles = new List<Form1.planar_object_store.face2d>();

            // Edge data
            foreach (edge_store edge in BW_delaunay.edges_data.all_edges)
            {
                point_store pt1 = BW_delaunay.points_data.get_point(edge.start_pt_id);
                point_store pt2 = BW_delaunay.points_data.get_point(edge.end_pt_id);

                Form1.planar_object_store.point2d pt1_o = new Form1.planar_object_store.point2d(pt1.pt_id, pt1.pt_coord.x, pt1.pt_coord.y);
                Form1.planar_object_store.point2d pt2_o = new Form1.planar_object_store.point2d(pt2.pt_id, pt2.pt_coord.x, pt2.pt_coord.y);

                Form1.planar_object_store.edge2d edge_o = new Form1.planar_object_store.edge2d(edge.edge_id, pt1_o, pt2_o);

                output_edges.Add(edge_o);
            }


            // Triangles data
            foreach (triangle_store tri in BW_delaunay.triangles_data.all_triangles)
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
            foreach (edge_store edge in BW_delaunay.edges_data.all_edges)
            {
                point_store pt1 = BW_delaunay.points_data.get_point(edge.start_pt_id);
                point_store pt2 = BW_delaunay.points_data.get_point(edge.end_pt_id);

                Form1.planar_object_store.point2d pt1_o = new Form1.planar_object_store.point2d(pt1.pt_id, pt1.pt_coord.x, pt1.pt_coord.y);
                Form1.planar_object_store.point2d pt2_o = new Form1.planar_object_store.point2d(pt2.pt_id, pt2.pt_coord.x, pt2.pt_coord.y);

                Form1.planar_object_store.edge2d edge_o = new Form1.planar_object_store.edge2d(edge.edge_id, pt1_o, pt2_o);

                output_edges.Add(edge_o);
            }


            // Triangles data
            foreach (triangle_store tri in BW_delaunay.triangles_data.all_triangles)
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
