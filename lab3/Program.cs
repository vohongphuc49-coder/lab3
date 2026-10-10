using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    internal class Program
    {
        public class Nguoi
        {
            public string HoTen { get; set; }
            public DateTime NgaySinh { get; set; }

            public Nguoi(string hoTen, DateTime ngaySinh)
            {
                HoTen = hoTen;
                NgaySinh = ngaySinh;
            }

            public virtual string LayThongTin()
            {
                return $"Họ tên: {HoTen}, Ngày sinh: {NgaySinh.ToShortDateString()}";
            }
        }
        public class SinhVien : Nguoi
        {
            public string MaSinhVien { get; set; }
            public string MaLop { get; set; }
            private double diemTrungBinh;

            public double DiemTrungBinh
            {
                get => diemTrungBinh;
                set
                {
                    if (value < 0 || value > 10)
                        throw new ArgumentException("Điểm trung bình phải từ 0 đến 10");
                    diemTrungBinh = value;
                }
            }

            public SinhVien(string maSV, string hoTen, DateTime ngaySinh, string maLop, double diemTB)
                : base(hoTen, ngaySinh)
            {
                MaSinhVien = maSV;
                MaLop = maLop;
                DiemTrungBinh = diemTB;
            }

            public string XepLoai()
            {
                if (DiemTrungBinh >= 8) return "Giỏi";
                if (DiemTrungBinh >= 6.5) return "Khá";
                if (DiemTrungBinh >= 5) return "Trung bình";
                return "Yếu";
            }

            public override string LayThongTin()
            {
                return $"Mã SV: {MaSinhVien}, Họ tên: {HoTen}, Lớp: {MaLop}, Điểm TB: {DiemTrungBinh}, Xếp loại: {XepLoai()}";
            }
        }
        public class QuanLySinhVien
        {
            private List<SinhVien> danhSach = new List<SinhVien>();

            public void Them(SinhVien sv)
            {
                if (danhSach.Any(s => s.MaSinhVien == sv.MaSinhVien))
                    throw new ArgumentException("Mã sinh viên đã tồn tại!");
                danhSach.Add(sv);
            }

            public void Xoa(string maSV)
            {
                var sv = TimTheoMa(maSV);
                if (sv != null) danhSach.Remove(sv);
                else Console.WriteLine("Không tìm thấy sinh viên để xóa!");
            }

            public void SuaDiem(string maSV, double diemMoi)
            {
                var sv = TimTheoMa(maSV);
                if (sv != null) sv.DiemTrungBinh = diemMoi;
                else Console.WriteLine("Không tìm thấy sinh viên để sửa điểm!");
            }

            public SinhVien TimTheoMa(string maSV) =>
                danhSach.FirstOrDefault(s => s.MaSinhVien == maSV);

            public List<SinhVien> TimTheoTen(string keyword) =>
                danhSach.Where(s => s.HoTen.Contains(keyword)).ToList();

            public List<SinhVien> SapXepTheoDiem() =>
                danhSach.OrderByDescending(s => s.DiemTrungBinh).ToList();

            public List<SinhVien> LocSinhVienDat() =>
                danhSach.Where(s => s.DiemTrungBinh >= 5).ToList();

            public List<SinhVien> LayDanhSach() => danhSach;
        }
        static void Main()
        {
            QuanLySinhVien qlsv = new QuanLySinhVien();
            int chon;

            do
            {
                Console.WriteLine("===== QUAN LY SINH VIEN =====");
                Console.WriteLine("1. Them sinh vien");
                Console.WriteLine("2. Xuat danh sach");
                Console.WriteLine("3. Tim sinh vien theo ma");
                Console.WriteLine("4. Tim sinh vien theo ten");
                Console.WriteLine("5. Sua diem trung binh");
                Console.WriteLine("6. Xoa sinh vien");
                Console.WriteLine("7. Sap xep theo diem giam dan");
                Console.WriteLine("8. Loc sinh vien dat");
                Console.WriteLine("0. Thoat");
                Console.Write("Chon chuc nang: ");

                if (!int.TryParse(Console.ReadLine(), out chon)) chon = -1;

                switch (chon)
                {
                    case 1:
                        try
                        {
                            Console.Write("Ma SV: ");
                            string ma = Console.ReadLine();
                            Console.Write("Ho ten: ");
                            string ten = Console.ReadLine();
                            Console.Write("Ngay sinh (yyyy-MM-dd): ");
                            DateTime ns = DateTime.Parse(Console.ReadLine());
                            Console.Write("Ma lop: ");
                            string lop = Console.ReadLine();
                            Console.Write("Diem TB: ");
                            double diem = double.Parse(Console.ReadLine());
                            qlsv.Them(new SinhVien(ma, ten, ns, lop, diem));
                            Console.WriteLine("Thêm thành công!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Lỗi: " + ex.Message);
                        }
                        break;

                    case 2:
                        foreach (var sv in qlsv.LayDanhSach())
                            Console.WriteLine(sv.LayThongTin());
                        break;

                    case 3:
                        Console.Write("Nhap ma SV: ");
                        var svMa = qlsv.TimTheoMa(Console.ReadLine());
                        Console.WriteLine(svMa != null ? svMa.LayThongTin() : "Không tìm thấy!");
                        break;

                    case 4:
                        Console.Write("Nhap tu khoa ten: ");
                        var dsTen = qlsv.TimTheoTen(Console.ReadLine());
                        foreach (var sv in dsTen) Console.WriteLine(sv.LayThongTin());
                        break;

                    case 5:
                        Console.Write("Nhap ma SV: ");
                        string maSua = Console.ReadLine();
                        Console.Write("Nhap diem moi: ");
                        if (double.TryParse(Console.ReadLine(), out double diemMoi))
                            qlsv.SuaDiem(maSua, diemMoi);
                        else Console.WriteLine("Điểm không hợp lệ!");
                        break;

                    case 6:
                        Console.Write("Nhap ma SV: ");
                        qlsv.Xoa(Console.ReadLine());
                        break;

                    case 7:
                        foreach (var sv in qlsv.SapXepTheoDiem())
                            Console.WriteLine(sv.LayThongTin());
                        break;

                    case 8:
                        foreach (var sv in qlsv.LocSinhVienDat())
                            Console.WriteLine(sv.LayThongTin());
                        break;
                }
            } while (chon != 0);
        }
    }
}