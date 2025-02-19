﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpSchool.Domain.Dtos;

namespace UpSchool.Domain.Utilities
{
    public class PasswordGenerator
    {
        private const string Numbers = "0123456789";
        private const string LowercaseCharacters = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string SpecialCharacters = "!@#$%^&*()";
        

        private readonly Random _random;
        private StringBuilder _passwordBuilder;
        private readonly StringBuilder _charSetBuilder;
        private readonly IIPHelper _ipHelper;
        private readonly ILocalDB _localDB;
        private int flag = 0;

        public PasswordGenerator(IIPHelper ipHelper, ILocalDB localDB)
        {
            _ipHelper = ipHelper;
            _localDB=localDB;

            _random = new Random();

            _passwordBuilder = new StringBuilder();

            _charSetBuilder = new StringBuilder();


            
        }
        public PasswordGenerator()
        {
            _random = new Random();

            _passwordBuilder = new StringBuilder();

            _charSetBuilder = new StringBuilder();

            flag=1;
        }

        public string Generate(GeneratePasswordDto generatePasswordDto)
        {
            if (flag != 1)
            {
                if (string.IsNullOrEmpty(_ipHelper.GetCurrentIPAddress()))
                {
                    throw new ArgumentNullException("IP Address is not valid.");
                }

                var ipList = _localDB.IPs;

                if (!ipList.Any())
                {
                    throw new ArgumentNullException("IP", "There are no IPs in the db to connect.");
                }
            }
            

            _charSetBuilder.Clear();
            _passwordBuilder.Clear();

            if (generatePasswordDto.IncludeNumbers) _charSetBuilder.Append(Numbers);

            if (generatePasswordDto.IncludeLowercaseCharacters) _charSetBuilder.Append(LowercaseCharacters);

            if (generatePasswordDto.IncludeUppercaseCharacters) _charSetBuilder.Append(UppercaseCharacters);

            if (generatePasswordDto.IncludeSpecialCharacters) _charSetBuilder.Append(SpecialCharacters);

            //if (!generatePasswordDto.IncludeNumbers && !generatePasswordDto.IncludeLowercaseCharacters &&
            //!generatePasswordDto.IncludeUppercaseCharacters && !generatePasswordDto.IncludeSpecialCharacters
            //    )
            if (generatePasswordDto is
                {
                    IncludeNumbers: false, IncludeLowercaseCharacters: false,
                    IncludeUppercaseCharacters: false, IncludeSpecialCharacters: false
                })
            {
                return string.Empty;
            }

            var charSet = _charSetBuilder.ToString();

            for (int i = 0; i < generatePasswordDto.Length; i++)
            {
                var randomIndex = _random.Next(charSet.Length);

                _passwordBuilder.Append(charSet[randomIndex]);
            }

            return _passwordBuilder.ToString();
        }

        public PasswordGeneratorMemento Save() => new PasswordGeneratorMemento
        {
            _passwordBuilderMemento=_passwordBuilder
        };

        public string undo(PasswordGeneratorMemento Memento)
        {

            _passwordBuilder=Memento._passwordBuilderMemento;

            return _passwordBuilder.ToString();
            
        }
    }
}
