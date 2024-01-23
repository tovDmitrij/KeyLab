﻿using component.v1.exceptions;
using service.v1.validation;

using Xunit;

namespace misc.unit_tests.Services
{
    public sealed class ValidationServiceTests
    {
        [Fact]
        public void ValidateEmail()
        {
            var validation = new ValidationService();
            var emails = new List<string>() { "", "sample", "12@mail", "@lol.", "@." };

            foreach (var email in emails)
            {
                Assert.Throws<BadRequestException>(() => validation.ValidateEmail(email));
            }
        }

        [Fact]
        public void ValidatePassword()
        {
            var validation = new ValidationService();
            var passwords = new List<string>() { "", "1234567", "1234567*&7hg" };

            foreach (var password in passwords)
            {
                Assert.Throws<BadRequestException>(() => validation.ValidatePassword(password));
            }
        }

        [Fact]
        public void ValidateNickname()
        {
            var validation = new ValidationService();
            var nicknames = new List<string>() { "", "Eu", "E76s8_$%" };

            foreach (var nickname in nicknames)
            {
                Assert.Throws<BadRequestException>(() => validation.ValidateNickname(nickname));
            }
        }

        [Fact]
        public void ValidateKeyboardTitle()
        {
            var validation = new ValidationService();
            var titles = new List<string>() { "", "Eu", "E76s8_$%" };

            foreach (var title in titles)
            {
                Assert.Throws<BadRequestException>(() => validation.ValidateKeyboardTitle(title));
            }
        }

        [Fact]
        public void ValidateKeyboardDescription()
        {
            var validation = new ValidationService();
            var descriptions = new List<string>() { "", "Eu", "E76s8_$%" };

            foreach (var description in descriptions)
            {
                Assert.Throws<BadRequestException>(() => validation.ValidateKeyboardTitle(description));
            }
        }
    }
}