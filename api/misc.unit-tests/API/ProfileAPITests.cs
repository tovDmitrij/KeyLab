using Xunit;

namespace misc.unit_tests.API
{
    public sealed class ProfileAPITests : APITest
    {
        [Fact]
        public void ProfileNickname_200()
        {
            var profileUrl = "http://127.0.0.1:6005/api/v1/profile/nickname";

        }

        [Fact]
        public void ProfileNickname_400() 
        { 
        
        }
    }
}