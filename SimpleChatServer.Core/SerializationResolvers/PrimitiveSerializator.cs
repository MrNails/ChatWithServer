using System;
using System.IO;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Exceptions;

namespace SimpleChatServer.Core.SerializationResolvers
{
    public enum Primitive : byte
    {
        Boolean,
        Int16,
        Int32,
        Int64,
        UInt16,
        UInt32,
        UInt64,
        Byte,
        SByte,
        Single,
        Double,
        Decimal,
        Char,
        CharArr,
        String,
        ByteArr
    }
    
    public class PrimitiveSerializator : ISerializator
    {
        public static readonly PrimitiveSerializator Serializator = new PrimitiveSerializator();
        
        public void Serialize(BinaryWriter writer, object data)
        {
            switch (data)
            {
                case bool @bool:
                    writer.Write((byte)Primitive.Boolean);
                    writer.Write(@bool);
                    break;
                case Int16 int16:
                    writer.Write((byte)Primitive.Int16);
                    writer.Write(int16);
                    break;
                case Int32 int32:
                    writer.Write((byte)Primitive.Int32);
                    writer.Write(int32);
                    break;
                case Int64 int64:
                    writer.Write((byte)Primitive.Int64);
                    writer.Write(int64);
                    break;
                case UInt16 uint16:
                    writer.Write((byte)Primitive.UInt16);
                    writer.Write(uint16);
                    break;
                case UInt32 uint32:
                    writer.Write((byte)Primitive.UInt32);
                    writer.Write(uint32);
                    break;
                case UInt64 uint64:
                    writer.Write((byte)Primitive.UInt64);
                    writer.Write(uint64);
                    break;
                case Byte @byte:
                    writer.Write((byte)Primitive.Byte);
                    writer.Write(@byte);
                    break;
                case SByte @sbyte:
                    writer.Write((byte)Primitive.SByte);
                    writer.Write(@sbyte);
                    break;
                case Single @float:
                    writer.Write((byte)Primitive.Single);
                    writer.Write(@float);
                    break;
                case Double @double:
                    writer.Write((byte)Primitive.Double);
                    writer.Write(@double);
                    break;
                case Char @char:
                    writer.Write((byte)Primitive.Char);
                    writer.Write(@char);
                    break;
                case String @string:
                    writer.Write((byte)Primitive.String);
                    writer.Write(@string);
                    break;
                case Char[] chars:
                    writer.Write((byte)Primitive.CharArr);
                    writer.Write(chars.Length);
                    writer.Write(chars);
                    break;
                case Decimal @decimal:
                    writer.Write((byte)Primitive.Decimal);
                    writer.Write(@decimal);
                    break;
                case Byte[] @bytes:
                    writer.Write((byte)Primitive.ByteArr);
                    writer.Write(@bytes.Length);
                    writer.Write(@bytes);
                    break;
                default:
                    var type = data.GetType();
                    throw new SerializationException(type, $"Cannot serialize type {type.FullName}");
            }

        }

        public object Deserialize(BinaryReader reader)
        {
            var primitive = (Primitive)reader.ReadByte();

            switch (primitive)
            {
                case Primitive.Boolean:
                    return reader.ReadBoolean();
                case Primitive.Int16:
                    return reader.ReadInt16();
                case Primitive.Int32:
                    return reader.ReadInt32();
                case Primitive.Int64:
                    return reader.ReadInt64();
                case Primitive.UInt16:
                    return reader.ReadUInt16();
                case Primitive.UInt32:
                    return reader.ReadUInt32();
                case Primitive.UInt64:
                    return reader.ReadUInt64();
                case Primitive.Byte:
                    return reader.ReadByte();
                case Primitive.SByte:
                    return reader.ReadSByte();
                case Primitive.Single:
                    return reader.ReadSingle();
                case Primitive.Double:
                    return reader.ReadDouble();
                case Primitive.Decimal:
                    return reader.ReadDecimal();
                case Primitive.Char:
                    return reader.ReadChar();
                case Primitive.CharArr:
                    return reader.ReadChars(reader.ReadInt32());
                case Primitive.String:
                    return reader.ReadString();
                case Primitive.ByteArr:
                    return reader.ReadBytes(reader.ReadInt32());
                default:
                    return null;
            }
        }
    }
}