using System;
using System.Runtime.InteropServices;

namespace Util
{
    public static class PlatecWarp
    {
        
        
        [DllImport("Platec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr platec_api_create(
            long seed, // 随机数生成的种子值，使用 long 数据类型表示。
            uint width, // 模拟的宽度。
            uint height, // 模拟的高度。
            float sea_level, // 海平面高度。
            uint erosion_period, // 侵蚀周期。
            float folding_ratio, // 折叠比例。
            uint aggr_overlap_abs, // 聚合重叠（绝对）。
            float aggr_overlap_rel, // 聚合重叠（相对）。
            uint cycle_count, // 循环计数。
            uint num_plates // 地板数量。
        );

        [DllImport("Platec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool platec_api_is_finished(IntPtr objectHandle);

        [DllImport("Platec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool platec_api_step(IntPtr objectHandle);

        [DllImport("Platec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void platec_api_destroy(IntPtr objectHandle);

        [DllImport("Platec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr platec_api_get_heightmap(IntPtr objectHandle);

        [DllImport("Platec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr platec_api_get_platesmap(IntPtr objectHandle);

        [DllImport("Platec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float platec_api_velocity_unity_vector_x(IntPtr objectHandle, uint plate_index);

        [DllImport("Platec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern float platec_api_velocity_unity_vector_y(IntPtr objectHandle, uint plate_index);

        [DllImport("Platec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint lithosphere_getMapWidth(IntPtr objectHandle);

        [DllImport("Platec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint lithosphere_getMapHeight(IntPtr objectHandle);
    }
}