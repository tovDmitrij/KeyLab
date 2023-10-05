using BenchmarkDotNet.Attributes;

using System.Text.RegularExpressions;

namespace misc.benchmarks.Benchmarks
{
    //https://youtu.be/WosEhlHATOk
    [MemoryDiagnoser]
    public partial class ValidationServiceBenchmark
    {
        [Params("ivanov@mail.ru", "ivanovmail.ru")]
        public string Email { get; set; }



        [GeneratedRegex(@"^[\w-.]+\@[\-\w]+\.[\w]+$")]
        private partial Regex EmailRgx();

        private readonly Regex _emailRgx;

        public ValidationServiceBenchmark()
        {
            _emailRgx = EmailRgx();
        }



        [Benchmark]
        public void Validate_OLD()
        {
            string pattern = @"^[\w-.]+\@[\-\w]+\.[\w]+$";
            var rgx = new Regex(pattern);
            rgx.IsMatch(Email);
        }

        [Benchmark]
        public void Validate_Mk1()
        {
            string pattern = @"^[\w-.]+\@[\-\w]+\.[\w]+$";
            var rgx = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            rgx.IsMatch(Email);
        }

        [Benchmark]
        public void Validate_Mk2()
        {
            EmailRgx().IsMatch(Email);
        }

        [Benchmark]
        public void Validate_Mk3()
        {
            _emailRgx.IsMatch(Email);
        }
    }
}