using BenchmarkDotNet.Attributes;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace misc.benchmarks
{
    [MemoryDiagnoser]
    public class SecurityServiceBenchmark
    {
        [Benchmark]
        public string HashPassword_OLD()
        {
            var salt = "h30yIKEJwRaqVVztZG5EiMLneWj93tW1";
            var password = "11111111";
            var bytes = Encoding.UTF8.GetBytes($"{salt}{password}");

            var hash = SHA512.HashData(bytes);

            var result = new StringBuilder();
            foreach (var b in hash)
            {
                result.Append(b.ToString("x2"));
            }
            return result.ToString();
        }

        [Benchmark]
        public string HashPassword_Mk1()
        {
            var salt = "h30yIKEJwRaqVVztZG5EiMLneWj93tW1";
            var password = "11111111";
            var bytes = Encoding.UTF8.GetBytes($"{salt}{password}");

            var hash = SHA512.HashData(bytes);

            ref var start = ref MemoryMarshal.GetArrayDataReference(hash);
            ref var end = ref Unsafe.Add(ref start, hash.Length);

            var result = new StringBuilder();
            while (Unsafe.IsAddressLessThan(ref start, ref end))
            {
                result.Append(start.ToString("x2"));
                start = ref Unsafe.Add(ref start, 1);
            }
            return result.ToString();
        }

        [Benchmark]
        public string HashPassword_Mk2()
        {
            var salt = "h30yIKEJwRaqVVztZG5EiMLneWj93tW1";
            var password = "11111111";
            var bytes = Encoding.UTF8.GetBytes($"{salt}{password}");

            var hash = SHA512.HashData(bytes).AsSpan();

            ref var start = ref MemoryMarshal.GetArrayDataReference(hash.ToArray());
            ref var end = ref Unsafe.Add(ref start, hash.Length);

            var result = new StringBuilder();
            while (Unsafe.IsAddressLessThan(ref start, ref end))
            {
                result.Append(start);
                start = ref Unsafe.Add(ref start, 1);
            }
            return result.ToString();
        }
    }
}