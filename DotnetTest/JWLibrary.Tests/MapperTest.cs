using Mapster;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Tests {
    public class MapperTest {
        [SetUp]
        public void Setup() {

        }

        [Test]
        public void MapsterTest1() {
            var add = new Address();
            add.Id = 1;
            add.Street = "test1";
            add.City = "seoul";
            add.Country = "south korea";
            var dto = add.Adapt<AddressDTO>();
            Assert.AreEqual(add.Id, dto.Id);
        }
    }

    public class Address {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class AddressDTO {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
    public struct GpsPosition {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public GpsPosition(double latitude, double longitude) {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }

    public class Customer {
        public Int32? Id { get; set; }
        public String Name { get; set; }
        public Address Address { get; set; }
        public Address HomeAddress { get; set; }
        public Address[] AddressList { get; set; }
        public IEnumerable<Address> WorkAddressList { get; set; }
    }

    public class CustomerDTO {
        public Int32? Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public AddressDTO HomeAddress { get; set; }
        public AddressDTO[] AddressList { get; set; }
        public List<AddressDTO> WorkAddressList { get; set; }
        public String AddressCity { get; set; }
    }
}
