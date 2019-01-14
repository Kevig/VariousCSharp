using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloxSoft.Classes
{
    /// <summary>
    /// Enum for specific contact number types
    /// </summary>
    public enum ContactNumberType
    {
        BUSINESS, PERSONAL, WORK, HOME, MOBILE, UNKNOWN
    }

    /// <summary>
    /// Contact Number Object
    /// </summary>
    public class ContactNumber : IComparable
    {
        /// <summary>
        /// ContactNumber Attributes
        /// </summary>
        /// <param name="anExt">Extention Code for Call Directing</param>
        /// <param name="aCode">Area Code / STD, Typically first 5 digits of a phone number</param>
        /// <param name="aNumber">Main Phone number, typically 6 digits</param>
        /// <param name="aType">Contact Number type, ie. Home, Mobile</param>
        /// 
        private string extention;
        private string code;
        private string number;

        private ContactNumberType type;



        // Class Constructors

        /// <summary>
        /// Construct Object with all atrributes null
        /// </summary>
        public ContactNumber()
        {
            this.extention = null;
            this.code = null;
            this.number = null;
            this.type = ContactNumberType.UNKNOWN;
        }

        /// <summary>
        /// Construct Object with Code and Number only, Extention null and Type unknown
        /// </summary>
        /// <param name="aCode"></param>
        /// <param name="aNumber"></param>
        public ContactNumber(string aCode, string aNumber)
        {
            this.extention = null;
            this.code = aCode;
            this.number = aNumber;
            this.type = ContactNumberType.UNKNOWN;
        }

        /// <summary>
        /// Construct Object with Code, Number and Type (Defined), Extention null.
        /// </summary>
        /// <param name="aCode"></param>
        /// <param name="aNumber"></param>
        /// <param name="aType"></param>
        public ContactNumber(string aCode, string aNumber, ContactNumberType aType)
        {
            this.extention = null;
            this.code = aCode;
            this.number = aNumber;
            this.setType(aType);
        }

        /// <summary>
        /// Construct Object with Code, Number and Type Index, Extention null.
        /// </summary>
        /// <param name="aCode"></param>
        /// <param name="aNumber"></param>
        /// <param name="aType">Integer Index value of ContactNumberType</param>
        public ContactNumber(string aCode, string aNumber, int aType)
        {
            this.extention = null;
            this.code = aCode;
            this.number = aNumber;
            this.setType(aType);
        }

        /// <summary>
        /// Construct Object with Extention, Code, Number and Type (Defined).
        /// </summary>
        /// <param name="anExt"></param>
        /// <param name="aCode"></param>
        /// <param name="aNumber"></param>
        /// <param name="aType"></param>
        public ContactNumber(string anExt, string aCode, string aNumber, ContactNumberType aType)
        {
            this.extention = anExt;
            this.code = aCode;
            this.number = aNumber;
            this.setType(aType);
        }

        /// <summary>
        /// Construct Object with Extention, Code, Number and Type Index.
        /// </summary>
        /// <param name="anExt"></param>
        /// <param name="aCode"></param>
        /// <param name="aNumber"></param>
        /// <param name="aType">Integer Index value of ContactNumberType</param>
        public ContactNumber(string anExt, string aCode, string aNumber, int aType)
        {
            this.extention = anExt;
            this.code = aCode;
            this.number = aNumber;
            this.setType(aType);
        }



        // Getters / Setters
        // Hard Typed for Extendability and / or Validations

        // Extention
        public void setExtention(string anExt)
        {
            this.extention = anExt;
        }

        public string getExtention()
        {
            return this.extention;
        }

        // Code
        public void setCode(string aCode)
        {
            this.code = aCode;
        }

        public string getCode()
        {
            return this.code;
        }

        // Number
        public void setNumber(string aNumber)
        {
            this.number = aNumber;
        }

        public string getNumber()
        {
            return this.number;
        }

        // Type
        public void setType(ContactNumberType aType)
        {
            this.type = aType;
        }

        public void setType(int aType)
        {
            this.type = this.indexToType(aType);
        }

        public ContactNumberType getType()
        {
            return this.type;
        }

        // Class Specific Utility

        /// <summary>
        /// Attempt to Convert Integer Index value into a defined ContactNumberType
        /// </summary>
        /// <param name="anInt">Integer Index value of ContactNumberType</param>
        /// <returns>A defined ContactNumberType if exists else Type.UNKNOWN</returns>
        private ContactNumberType indexToType(int anInt)
        {
            try
            {
                ContactNumberType aType = (ContactNumberType)anInt;
                return aType;
            }
            catch(Exception e)
            {
                // Handle - Enum type of Index given does not exist
            }
            return ContactNumberType.UNKNOWN;
        }



        // Sorting Helpers

        /// <summary>
        /// Alphabetical Sort by Type Value
        /// </summary>
        private class SortByTypeAscending : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                ContactNumber c1 = (ContactNumber)a;
                ContactNumber c2 = (ContactNumber)b;

                int n = string.Compare(c1.getType().ToString(), c2.getType().ToString());
                return n;
            }
        }
        
        /// <summary>
        /// Alphabetical Sort by Code Value
        /// </summary>
        private class SortByCodeAscending : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                ContactNumber c1 = (ContactNumber)a;
                ContactNumber c2 = (ContactNumber)b;

                int n = string.Compare(c1.getCode(), c2.getCode());
                return n;
            }
        }

        /// <summary>
        /// Default Alphabetical Sort by Number Value
        /// </summary>
        int IComparable.CompareTo(object obj)
        {
            ContactNumber c = (ContactNumber)obj;
            return string.Compare(this.number, c.getNumber());
        }



        // Sort Type Methods

        // Implementation of sort by type for IComparable .Sort() function
        public static IComparer sortTypeAscending()
        {
            return (IComparer)new SortByTypeAscending();
        }

        // Implementation of sort by code for IComparable .Sort() function
        public static IComparer sortCodeAscending()
        {
            return (IComparer)new SortByCodeAscending();
        }

    }
}