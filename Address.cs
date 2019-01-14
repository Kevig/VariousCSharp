using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloxSoft.Classes
{

    public enum PropertyType
    {
        UNKNOWN = 0,
        FLAT = 1,
        BUNGALOW = 2,
        TERRACED = 3,
        DETACHED = 4,
        SEMI_DETACHED = 5,
        END_TERRACE = 6,
        COTTAGE = 7
    }

    public enum TenancyType
    {
        UNKNOWN = 0,
        OWNED = 1,
        PRIVATE_RENTING = 2,
        COUNCIL_RENTING = 3
    }

    public enum AddressType
    {
        UNKNOWN = 0,
        BUSINESS = 1,
        RESIDENTIAL = 2,
        INDUSTRIAL = 3
    }

    public class Address : IComparable
    {
        // Attributes
        private string name;
        private string[] line;
        private string postCode;

        private PropertyType propertyType;
        private TenancyType tenancy;
        private AddressType addressType;

        // Get / Set

        // House Name / Number

        public void setName(string aName)
        {
            this.name = aName;
        }

        public string getName()
        {
            return this.name;
        }

        // Address
        
        public void setLine(int lineRef, string aLine)
        {
            try
            {
                this.line[lineRef] = aLine;
            }
            catch(Exception e)
            {
                // Handle index exception
            }
        }

        public string getLine(int lineRef)
        {
            return this.line[lineRef];
        }

        // Postcode

        public void setPostCode(string aPostCode)
        {
            this.postCode = aPostCode;
        }

        public string getPostCode()
        {
            return this.postCode;
        }

        // Property type

        public void setPropertyType(PropertyType aType)
        {
            this.propertyType = aType;
        }

        public void setPropertyType(int i)
        {
            this.propertyType = this.indexToPropertyType(i);
        }

        private PropertyType indexToPropertyType(int i)
        {
            try
            {
                PropertyType aType = (PropertyType)i;
                return aType;
            }
            catch(Exception e)
            {
                // Handle invalid index value
            }
            return PropertyType.UNKNOWN;
        }

        public PropertyType getPropertyType()
        {
            return this.propertyType;
        }
        
        public int getPropertyTypeIndex(PropertyType aType)
        {
            return (int)aType;
        }

        // Tenancy type

        public void setTenancyType(TenancyType aType)
        {
            this.tenancy = aType;
        }

        public void setTenancyType(int i)
        {
            this.tenancy = this.indexToTenancyType(i);
        }

        private TenancyType indexToTenancyType(int i)
        {
            try
            {
                TenancyType aType = (TenancyType)i;
                return aType;
            }
            catch(Exception e)
            {
                // Handle invalid index value
            }
            return TenancyType.UNKNOWN;
        }

        public TenancyType getTenancyType()
        {
            return this.tenancy;
        }

        public int getTenancyTypeIndex(TenancyType aType)
        {
            return (int)aType;
        }

        // Address type
        public void setAddressType(AddressType aType)
        {
            this.addressType = aType;
        }

        public void setAddressType(int i)
        {
            this.addressType = this.indexToAddressType(i);
        }

        private AddressType indexToAddressType(int i)
        {
            try
            {
                AddressType aType = (AddressType)i;
                return aType;
            }
            catch(Exception e)
            {
                // Handle invalid index value
            }
            return AddressType.UNKNOWN;
        }

        public AddressType getAddressType()
        {
            return this.addressType;
        }

        public int getAddressTypeIndex(AddressType aType)
        {
            return (int)aType;
        }

        
    }
}