﻿using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ModBus.Net
{
    /// <summary>
    /// 数据单元翻译器
    /// </summary>
    public abstract class AddressTranslator
    {
        protected static AddressTranslator _instance;

        public static AddressTranslator Instance
        {
            get
            {
                if (_instance == null)
                {
                    CreateTranslator(new AddressTranslatorBase());
                }
                return _instance;
            }
            protected set
            {
                if (value == null) CreateTranslator(new AddressTranslatorBase());
                _instance = value;
            }
        }

        public static void CreateTranslator(AddressTranslator instance)
        {
            Instance = instance;
        }

        public abstract KeyValuePair<int,int> AddressTranslate(string address, bool isRead);
    }

    /// <summary>
    /// NA200H数据单元翻译器
    /// </summary>
    public class AddressTranslatorNA200H : AddressTranslator
    {
        protected Dictionary<string, int> TransDictionary;
        protected Dictionary<string, int> ReadFunctionCodeDictionary;
        protected Dictionary<string, int> WriteFunctionCodeDictionary;
 
        public AddressTranslatorNA200H()
        {
            TransDictionary = new Dictionary<string, int>
            {
                {"Q", 0},
                {"M", 10000},
                {"N", 20000},
                {"I", 0},
                {"S", 10000},
                {"IW", 0},
                {"SW", 5000},
                {"MW", 0},
                {"NW", 10000},
                {"QW", 20000},
            };
            ReadFunctionCodeDictionary = new Dictionary<string, int>
            {
                {"Q", (int)ModbusProtocalReadDataFunctionCode.ReadCoilStatus},
                {"M", (int)ModbusProtocalReadDataFunctionCode.ReadCoilStatus},
                {"N", (int)ModbusProtocalReadDataFunctionCode.ReadCoilStatus},
                {"I", (int)ModbusProtocalReadDataFunctionCode.ReadInputStatus},
                {"S", (int)ModbusProtocalReadDataFunctionCode.ReadInputStatus},
                {"IW", (int)ModbusProtocalReadDataFunctionCode.ReadInputRegister},
                {"SW", (int)ModbusProtocalReadDataFunctionCode.ReadInputRegister},
                {"MW", (int)ModbusProtocalReadDataFunctionCode.ReadHoldRegister},
                {"NW", (int)ModbusProtocalReadDataFunctionCode.ReadHoldRegister},
                {"QW", (int)ModbusProtocalReadDataFunctionCode.ReadHoldRegister},
            };
            WriteFunctionCodeDictionary = new Dictionary<string, int>
            {
                {"Q", (int)ModbusProtocalWriteDataFunctionCode.WriteMultiCoil},
                {"M", (int)ModbusProtocalWriteDataFunctionCode.WriteMultiCoil},
                {"N", (int)ModbusProtocalWriteDataFunctionCode.WriteMultiCoil},
                {"MW", (int)ModbusProtocalWriteDataFunctionCode.WriteMultiRegister},
                {"NW", (int)ModbusProtocalWriteDataFunctionCode.WriteMultiRegister},
                {"QW", (int)ModbusProtocalWriteDataFunctionCode.WriteMultiRegister},
            };
        }

        public override KeyValuePair<int, int> AddressTranslate(string address, bool isRead)
        {
            address = address.ToUpper();
            int i = 0;
            int t;
            while (!int.TryParse(address[i].ToString(), out t) && i < address.Length)
            {
                i++;
            }
            if (i == 0 || i >= address.Length) throw new FormatException();
            string head = address.Substring(0, i);
            string tail = address.Substring(i);
            return isRead
                ? new KeyValuePair<int, int>(TransDictionary[head] + int.Parse(tail) - 1,
                    ReadFunctionCodeDictionary[head])
                : new KeyValuePair<int, int>(TransDictionary[head] + int.Parse(tail) - 1,
                    WriteFunctionCodeDictionary[head]);
        }
    }

    /// <summary>
    /// 基本的单元转换器
    /// </summary>
    public class AddressTranslatorBase : AddressTranslator
    {
        public override KeyValuePair<int, int> AddressTranslate(string address, bool isRead)
        {
            int num1,num2;
            string[] split = address.Split(':');
            if (split.Length != 2) throw new FormatException();
            if (int.TryParse(split[0], out num1) && int.TryParse(split[1], out num2))
            {
                return new KeyValuePair<int, int>(num2, num1);
            }
            throw new FormatException();
        }
    }
}