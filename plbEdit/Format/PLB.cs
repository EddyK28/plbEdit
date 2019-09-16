using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace plbEdit.Format
{
    //NOTE: all strings start on 8 byte boundaries (0x0, 0x8)
    //NOTE: structs and lists start on 4 byte boundaries?
    //NOTE: unused/empty lists have offset they would start at if used
    //NOTE: Different Item types have extra bytes
    //  RangeType, SplineType + 12B
    //  CameraAtType + 8B
    //  GimmickArtType, CharaType, CameraEyeType + 4B

    //TODO: Change public members to protected + properties?

    //Raw PLB Header Struct
    public struct PLBHeaderRaw
    {
        public uint pointerListSize;
        public uint pointerListOfs;
        public uint ukn0;               // = 1?

        public uint uknListSize;        //unkown list, always empty, but size 2?
        public uint uknListOfs;         //  always at 0x70?
        public uint ukn1;               // = 1?

        public uint sectionListSize;
        public uint sectionListOfs;

        public uint groupListSize;
        public uint groupListOfs;

        public uint layerListSize;
        public uint layerListOfs;

        //NOTE: unused/empty lists have offset same as Pointer List ??
    }

    //Raw Section Header Struct
    public struct SectionHeaderRaw
    {
        public uint nameOfs;

        public uint groupRefListSize;
        public uint groupRefListOfs;

        public uint itemListSize;
        public uint itemListOfs;

        public uint mapListSize;
        public uint mapListOfs;

        public uint pad;       // = 0?
    }

    //Raw Group Reference Struct
    public struct GroupRefRaw
    {
        public uint ukn;
        public uint ofs;
    }

    //Raw Item Entry Header Struct
    public struct ItemEntryRaw
    {
        public uint pad;
        public uint stringTypeOfs;
        public uint stringIdOfs;
        public uint stringLabelOfs;
        public uint layerOfs;
        public float posX;
        public float posY;
        public float posZ;
        public uint ukn1;
        public uint ukn2;
    }

    //Raw Map Entry Header Struct
    public struct MapEntryRaw
    {
        public uint pad;
        public uint typeOfs;
        public uint idOfs;
        public uint labelOfs;
        public uint ukn1;
        public uint ukn2;
        public uint ukn3;
        public uint ukn4;
    }

    //Raw Group Header Struct
    public struct GroupHeaderRaw
    {
        public uint typeOfs;
        public uint idOfs;

        public uint sectionListSize;
        public uint sectionListOfs;

        public uint itemListSize;
        public uint itemListOfs;

        public uint uknListSize;
        public uint uknListOfs;

        public uint pad;
    }

    public struct GroupSectionHeaderRaw
    {
        public uint stringTypeOfs;
        public uint stringIdOfs;

        public uint groupListSize;
        public uint groupListOfs;

        public uint itemListSize;
        public uint itemListOfs;

        public uint mapListSize;
        public uint mapListOfs;

        public uint pad;       // = 0?
    }

    //Raw Layer Header Struct
    public struct LayerHeaderRaw
    {
        public uint pad;
        public uint nameOfs;
        public uint data1;
        public uint data2;
        public uint data3;
        public uint data4;
        public uint ukn1;
        public uint ukn2;
    }

    [DataContract]
    public class PLB
    {
        //protected uint[] pointers;
        [DataMember(Order = 0, EmitDefaultValue = false)]
        protected Section[] sections;
        [DataMember(Order = 1, EmitDefaultValue = false)]
        protected Group[] groups;
        [DataMember(Order = 2, EmitDefaultValue = false)]
        protected Layer[] layers;

        protected uint[] buildData;

        public Section[] Sections { get { return sections; } }
        public Group[] Groups { get { return groups; } }
        public Layer[] Layers { get { return layers; } }

        public PLB(PLBReader plbReader)
        {
            //bValid = false;

            //TODO: better check for "PlaceDat\x00\x01"
            plbReader.BaseStream.Position = 0;
            if (plbReader.ReadUInt64() != 0x7461446563616C50)
                throw new FormatException("Bad PLB format");    
                //MessagePrinter.AddMsg($"Warning: This is probably not a PLB");
            

            GCHandle handle;
            int readSize;
            byte[] readBuffer;

            //Read PLB header data
            PLBHeaderRaw header;
            readSize = Marshal.SizeOf(typeof(PLBHeaderRaw));
            readBuffer = new byte[readSize];
            plbReader.BaseStream.Position = 0x40;   //Start of actual data
            readBuffer = plbReader.ReadBytes(readSize);
            handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
            header = (PLBHeaderRaw)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PLBHeaderRaw));
            handle.Free();

            //alert if ukns != 1
            if (header.ukn0 != 1)
                MessagePrinter.AddMsg($"Warning: Header Ukn0 is 0x{header.ukn0:X}, not 1");
            if (header.ukn1 != 1)
                MessagePrinter.AddMsg($"Warning: Header Ukn1 is 0x{header.ukn1:X}, not 1");

            //alert if uknList not normal (2,0x70)
            if (header.uknListSize != 2 || header.uknListOfs != 0x70)
                MessagePrinter.AddMsg($"Warning: Header Ukn List is 0x{header.uknListSize:X} - 0x{header.uknListOfs:X8}, not 0x2 - 0x00000070");


            //Read pointer list (not really needed, but good for testing)
            //pointers = new uint[header.pointerListSize];
            //binReader.BaseStream.Position = header.pointerListOfs;
            //for (uint i = 0; i < header.pointerListSize; i++)
            //{
            //    pointers[i] = binReader.ReadUInt32();
            //}

            //Read Layers (do this first as layers are referenced by other items)
            if (header.layerListSize > 0)
                Util.LoadListEntries<LayerHeaderRaw, Layer>(plbReader, header.layerListOfs, header.layerListSize, ref layers);

            //Read Groups
            if (header.groupListSize > 0)
            {
                Util.LoadListEntries<GroupHeaderRaw, Group>(plbReader, header.groupListOfs, header.groupListSize, ref groups);
            }

            //Read Sections
            if (header.sectionListSize > 0)
            {
                Util.LoadListEntries<SectionHeaderRaw, Section>(plbReader, header.sectionListOfs, header.sectionListSize, ref sections);
                if (header.groupListSize > 0)   //also decode groupRefs if there are groups
                {
                    List<uint> groupList = new List<uint>();
                    Util.ReadList(plbReader, groupList, header.groupListOfs, header.groupListSize);
                }
            }

            //bValid = true;
        }

        public uint BuildPtrs(PLBWriter plbWriter)
        {
            buildData = new uint[5];
            buildData[0] = 0;           //Pointer List Addr (temporarily 0)
            buildData[1] = 0x70;        //Ukn list Addr  (list is always empty?)
            buildData[2] = 0x74;        //Section list Addr  (may need calculating)

            plbWriter.PtrQueue(0x44);   //QUEUE: pos of PLB header pointer list ptr (fixed location)
            plbWriter.PtrQueue(0x50);   //QUEUE: pos of PLB header ukn list ptr (fixed location)

            //Build Section pointers
            uint tempOffs = buildData[2];
            uint tempHead = 0;
            if (sections != null)
            {
                //start of first section (start of list + list length + total header lengths)
                tempOffs += (uint)sections.Length * (Util.sizeSectionHeader + 4);
                tempHead = buildData[2] + (uint)sections.Length * 4;

                for (uint i = 0; i<sections.Length; i++)
                {
                    tempOffs = sections[i].BuildPtrs(plbWriter, Util.Align(tempOffs, 8), tempHead);
                    plbWriter.PtrQueue(buildData[2] + i * 4);   //QUEUE: pos of pointer to this section header in section list
                    tempHead += Util.sizeSectionHeader;
                }
            }

            plbWriter.PtrQueue(0x5C);   //QUEUE: pos of PLB header section list ptr (fixed location)


            buildData[3] = Util.Align(tempOffs, 4);     //Group list Addr

            //Build Group pointers
            tempOffs = buildData[3];
            if (groups != null)
            {
                //start of first group (same idea as sections)
                tempOffs += (uint)groups.Length * (Util.sizeGroupHeader + 4);
                tempHead = buildData[3] + (uint)groups.Length * 4;

                for (uint i = 0; i < groups.Length; i++)
                {
                    tempOffs = groups[i].BuildPtrs(plbWriter, Util.Align(tempOffs, 8), tempHead);
                    plbWriter.PtrQueue(buildData[3] + i * 4);   //QUEUE: pos of pointer to this Group in Group list
                    tempHead += Util.sizeGroupHeader;
                }
            }

            plbWriter.PtrQueue(0x64);   //QUEUE: pos of PLB header Group list ptr (fixed location)


            buildData[4] = Util.Align(tempOffs, 4);     //Layer list Addr

            //Build Layer pointers
            tempOffs = buildData[4];
            if (layers != null)
            {
                tempOffs += (uint)layers.Length * (Util.sizeLayerHeader + 4);
                tempHead = buildData[4] + (uint)layers.Length * 4;

                for (uint i = 0; i < layers.Length; i++)
                {
                    tempOffs = layers[i].BuildPtrs(plbWriter, Util.Align(tempOffs, 8), tempHead);
                    plbWriter.PtrQueue(buildData[4] + i * 4);   //QUEUE: pos of pointer to this layer in layer list
                    tempHead += Util.sizeLayerHeader;
                }
            }

            plbWriter.PtrQueue(0x6C);   //QUEUE: pos of PLB header Layer list ptr (fixed location)


            buildData[0] = Util.Align(tempOffs, 4);     //Pointer list Addr

            return 0;
        }

        public void Build(PLBWriter plbWriter)
        {
            BuildPtrs(plbWriter);

            //write "PlaceDat\x00\x01"
            plbWriter.Write(0x7461446563616C50);  
            plbWriter.Write((short)0x0100);

            plbWriter.BaseStream.Position = 0x40;

            //write PLB header
            plbWriter.Write(plbWriter.PtrCount());  //pointer list
            plbWriter.Write(buildData[0]);

            plbWriter.Write(1);                     //ukn

            plbWriter.Write(2);                     //ukn list
            plbWriter.Write(buildData[1]);

            plbWriter.Write(1);                     //ukn

            plbWriter.Write(sections?.Length ?? 0); //Section list
            plbWriter.Write(buildData[2]);

            plbWriter.Write(groups?.Length ?? 0);   //Group list
            plbWriter.Write(buildData[3]);

            plbWriter.Write(layers?.Length ?? 0);   //Layer list
            plbWriter.Write(buildData[4]);

            //write 4B pad (ukn list technically?)
            plbWriter.Write(0);

            //Store layer header start address
            if (layers != null)
                plbWriter.layerHeadAddr = buildData[4] + (uint)layers.Length*4;

            //Write Section data
            if (sections != null)
            {
                //write section header list (simple ptr calc)   
                Util.WriteList(plbWriter, buildData[2], sections.Length, Util.sizeSectionHeader);

                //write section headers (get data from section objects' buildData)
                foreach (Section section in sections)
                    section.BuildHeader(plbWriter);

                //build each section
                foreach (Section section in sections)
                    section.Build(plbWriter);
            }

            //Write Group data
            if (groups != null)
            {
                //write section header list (simple ptr calc)   
                Util.WriteList(plbWriter, buildData[3], groups.Length, Util.sizeGroupHeader);

                //write Group headers
                foreach (Group group in groups)
                    group.BuildHeader(plbWriter);

                //build each group
                foreach (Group group in groups)
                    group.Build(plbWriter);
            }

            //Write Layer data
            if (layers != null)
            {
                //write layer header list (simple ptr calc)   
                Util.WriteList(plbWriter, buildData[4], layers.Length, Util.sizeLayerHeader);

                //write layer headers (get data from section objects' buildData)
                foreach (Layer layer in layers)
                    layer.BuildHeader(plbWriter);

                //build each layer
                foreach (Layer layer in layers)
                    layer.Build(plbWriter);
            }

            //Write pointers
            Util.Align(plbWriter, 4);
            plbWriter.PtrWrite();

            return;
        }
    }

    [DataContract]
    public class Section
    {
        [DataMember(Order = 0)]
        protected string name;

        [DataMember(Order = 1, EmitDefaultValue = false)]
        protected GroupRef[] groupRefs;
        [DataMember(Order = 2, EmitDefaultValue = false)]
        protected ItemEntry[] items;
        [DataMember(Order = 3, EmitDefaultValue = false)]
        protected MapEntry[] maps;

        protected uint[] buildData;

        public string Name
        {
            get { return name; }
            set
            {
                name = value ?? throw new ArgumentNullException("name");
            }
        }

        public GroupRef[] GroupRefs { get { return groupRefs; } }
        public ItemEntry[] Items { get { return items; } }
        public MapEntry[] Maps { get { return maps; } }

        public Section(PLBReader plbReader, ref SectionHeaderRaw section, uint headPos)
        {
            //Read Section Name
            name = Util.ReadString(plbReader, section.nameOfs);

            //Read Group Ref List
            if (section.groupRefListSize > 0)
            {
                uint groupOfs = 0;
                groupRefs = new GroupRef[section.groupRefListSize];
                for (uint i = 0; i < section.groupRefListSize; i++)
                {
                    plbReader.BaseStream.Position = section.groupRefListOfs + i * 4;
                    plbReader.BaseStream.Position = plbReader.ReadUInt32();

                    groupRefs[i] = new GroupRef();
                    groupRefs[i].ukn = plbReader.ReadUInt32();

                    //Store group reference by group name
                    groupOfs = plbReader.ReadUInt32();
                    plbReader.BaseStream.Position = groupOfs + 4;
                    if (groupOfs == 0)
                        groupRefs[i].gref = "$NONE";
                    else
                    {
                        groupRefs[i].gref = plbReader.GroupUnique(Util.ReadString(plbReader, plbReader.ReadUInt32()), groupOfs);
                        //plbReader.GroupUnique()
                    }
                }
            }

            //Read Item List
            if (section.itemListSize > 0)
                Util.LoadListEntries<ItemEntryRaw, ItemEntry>(plbReader, section.itemListOfs, section.itemListSize, ref items);

            //Read Map List
            if (section.mapListSize > 0)
                Util.LoadListEntries<MapEntryRaw, MapEntry>(plbReader, section.mapListOfs, section.mapListSize, ref maps);
        }

        //Given start of section, caclulate pointers to contents and return end of section
        public uint BuildPtrs(PLBWriter plbWriter, uint offset, uint header)
        {
            buildData = new uint[4];
            buildData[0] = offset;                                              //Starting position of this section (after header)
            buildData[1] = Util.Align(offset + (uint)(name.Length+1) * 2, 4);   //Position of groupRef List
            buildData[2] = buildData[1] + (uint)(groupRefs?.Length ?? 0)*12;    //Position of item list

            plbWriter.PtrQueue(header);       //QUEUE: Section name pointer pos

            //Build pointers for groupRefs
            uint tempOffs = buildData[1];
            if (groupRefs != null)
            {
                tempOffs += (uint)groupRefs.Length*4 + 4;
                for (uint i=0; i<groupRefs.Length; i++)
                {
                    plbWriter.PtrQueue(tempOffs + i * 8);     //QUEUE: group pointer pos
                    plbWriter.PtrQueue(buildData[1] + i * 4); //QUEUE: pos of pointer to this in groupRef list
                }
            }
            plbWriter.PtrQueue(header + 8);     //QUEUE: pos of groupRef list pointer in section header

            //Build pointers for items
            tempOffs = buildData[2];
            uint tempHead = 0;
            if (items != null)
            {
                tempOffs += (uint)items.Length * (Util.sizeItemEntry + 4);      //end of item headers 
                tempHead = buildData[2] + (uint)items.Length * 4;
                foreach (ItemEntry item in items)                               //add space for extra data based on item type (it's in the headers)
                    tempOffs += item.GetExtraBytes();
                for (uint i = 0; i < items.Length; i++)                         //build pointers and get start addr of next item 
                {
                    tempOffs = items[i].BuildPtrs(plbWriter, Util.Align(tempOffs, 8), tempHead);
                    plbWriter.PtrQueue(buildData[2] + i * 4);     //QUEUE: pos of pointer to this in item list
                    tempHead += Util.sizeItemEntry + items[i].GetExtraBytes();
                }
            }
            plbWriter.PtrQueue(header + 16);     //QUEUE: pos of item list pointer in section header

            buildData[3] = Util.Align(tempOffs, 4);                             //Position of map list

            //Build string pointers for maps
            tempOffs = buildData[3];
            if (maps != null) 
            {
                tempOffs += (uint)maps.Length * (Util.sizeMapEntry + 4);       //end of map headers
                tempHead = buildData[3] + (uint)maps.Length * 4;
                for (uint i = 0; i < maps.Length; i++)                         //build pointers and get start addr of next map 
                {
                    tempOffs = maps[i].BuildPtrs(plbWriter, Util.Align(tempOffs, 8), tempHead);
                    plbWriter.PtrQueue(buildData[3] + i * 4);     //QUEUE: pos of pointer to this in map list
                    tempHead += Util.sizeMapEntry;
                }
            }
            plbWriter.PtrQueue(header + 24);     //QUEUE: pos of map list pointer in section header

            return tempOffs;
        }

        //Write section header using given PLBWriter
        public void BuildHeader(BinaryWriter binWriter)
        {
            binWriter.Write(buildData[0]);              //name string ptr
            binWriter.Write(groupRefs?.Length ?? 0);    //groupRef list size
            binWriter.Write(buildData[1]);              //groupRef list ptr
            binWriter.Write(items?.Length ?? 0);        //item list size
            binWriter.Write(buildData[2]);              //item list ptr
            binWriter.Write(maps?.Length ?? 0);         //map list size
            binWriter.Write(buildData[3]);              //map list ptr
            binWriter.Write(0);
        }

        //Write section contents using given PLBWriter
        public void Build(PLBWriter plbWriter)
        {
            //write pad to 8B
            Util.Align(plbWriter, 8);

            //write section name + pad to 4B
            Util.WriteString(plbWriter, name);
            Util.Align(plbWriter, 4);

            //Write groupRef Data
            if (groupRefs != null)
            {
                //write groupRef List
                Util.WriteList(plbWriter, buildData[1], groupRefs.Length, Util.sizeGroupRef);

                //write gropRefs
                foreach (GroupRef group in groupRefs)
                {
                    plbWriter.Write(group.ukn);
                    plbWriter.Write(plbWriter.GroupGetPtr(group.gref));
                }
            }
            
            //Write item Data
            if (items != null)
            {
                //write itemList
                uint temp = buildData[2] + (uint)items.Length * 4;
                plbWriter.Write(temp);
                for (uint i = 1; i < items.Length; i++)
                {
                    temp += items[i - 1].GetExtraBytes();
                    plbWriter.Write(temp + i * 40);
                }

                //write items headers
                foreach (ItemEntry item in items)
                    item.BuildHeader(plbWriter);

                //write item strings
                foreach (ItemEntry item in items)
                    item.Build(plbWriter);
            }

            //write pad to 4B
            Util.Align(plbWriter, 4);

            //Write map Data
            if (maps != null)
            {
                //write map List
                Util.WriteList(plbWriter, buildData[3], maps.Length, Util.sizeMapEntry);

                //write map headers
                foreach (MapEntry map in maps)
                    map.BuildHeader(plbWriter);

                //write map strings
                foreach (MapEntry map in maps)
                    map.Build(plbWriter);
            }

            //write pad to 4B
            Util.Align(plbWriter, 4);
        }
    }

    [DataContract]
    public class GroupRef
    {
        [DataMember(Order = 1)]
        public uint ukn;
        [DataMember(Order = 0, Name = "group")]
        public string gref;
    }

    [DataContract]
    public class ItemEntry
    {
        [DataMember(Order = 0)]
        protected string type;
        [DataMember(Order = 1)]
        protected string id;
        [DataMember(Order = 2)]
        protected string label;

        [DataMember(Order = 3)]
        public int layerIdx;

        [DataMember(Order = 4)]
        public float posX;
        [DataMember(Order = 4)]
        public float posY;
        [DataMember(Order = 4)]
        public float posZ;

        [DataMember(Order = 5)]
        public uint ukn1;
        [DataMember(Order = 5)]
        public uint ukn2;

        [DataMember(Order = 6, EmitDefaultValue = false)]
        protected uint[] extraData;

        protected uint[] buildData;


        public string Type
        {
            get { return type; }
            set
            {
                type = value ?? throw new ArgumentNullException("Type");
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                id = value ?? throw new ArgumentNullException("Id");
            }
        }

        public string Label
        {
            get { return label; }
            set
            {
                label = value ?? throw new ArgumentNullException("Label");
            }
        }

        public ItemEntry(PLBReader plbReader, ref ItemEntryRaw entry, uint headPos)
        {
            posX = entry.posX;
            posY = entry.posY;
            posZ = entry.posZ;

            layerIdx = plbReader.FindLayerAddr(entry.layerOfs);
            ukn1 = entry.ukn1;
            ukn2 = entry.ukn2;

            type = Util.ReadString(plbReader, entry.stringTypeOfs);
            id = Util.ReadString(plbReader, entry.stringIdOfs);
            label = Util.ReadString(plbReader, entry.stringLabelOfs);

            if (type == "RangeType" || type == "SplineType")
            {
                extraData = new uint[3];    //3 words range/spline data
                plbReader.BaseStream.Position = headPos + Util.sizeItemEntry;
                for (int i = 0; i < 3; i++)
                    extraData[i] = plbReader.ReadUInt32();
            }
            else if (type == "CameraAtType")
            {
                extraData = new uint[2];    //2 words of Uknown
                plbReader.BaseStream.Position = headPos + Util.sizeItemEntry;
                for (int i = 0; i < 2; i++)
                    extraData[i] = plbReader.ReadUInt32();
            }
            else if (type == "GimmickArtType" || type == "CharaType" || type == "CameraEyeType")
            {
                extraData = new uint[1];    //1 word of Uknown
                plbReader.BaseStream.Position = headPos + Util.sizeItemEntry;
                extraData[0] = plbReader.ReadUInt32();
            }

            //TODO: handle extra spline data
            if (type == "SplineType")
                MessagePrinter.AddMsg($"Warning: Skipping SplineType Data at 0x{extraData[2]:X8}");
        }

        public uint GetExtraBytes()
        {
            return (uint)(extraData?.Length ?? 0) * 4;
        }

        //Given start of item, caclulate pointers and return end of item
        public uint BuildPtrs(PLBWriter plbWriter, uint offset, uint header)
        {
            buildData = new uint[3];
            buildData[0] = offset;                                                  //String 1 Addr
            buildData[1] = Util.Align(offset + (uint)(type.Length+1) * 2, 8);       //String 2 Addr
            buildData[2] = Util.Align(buildData[1] + (uint)(id.Length+1) * 2, 8);   //String 3 Addr

            plbWriter.PtrQueue(header + 4);                                         //QUEUE: string 1 pointer pos
            plbWriter.PtrQueue(header + 8);                                         //QUEUE: string 2 pointer pos
            plbWriter.PtrQueue(header + 12);                                        //QUEUE: string 3 pointer pos
            plbWriter.PtrQueue(header + 16);                                        //QUEUE: layer pointer pos

            return buildData[2] + (uint)(label.Length+1) * 2;                       //Return end of strings
        }

        public void BuildHeader(PLBWriter plbWriter)
        {
            //write item header
            plbWriter.Write(0);
            plbWriter.Write(buildData[0]);              //type string ptr
            plbWriter.Write(buildData[1]);              //ID string ptr
            plbWriter.Write(buildData[2]);              //label string ptr

            //layer offset
            if (layerIdx == -1)
                plbWriter.Write(0); 
            else
                plbWriter.Write((uint)(plbWriter.layerHeadAddr + layerIdx * Util.sizeLayerHeader)); 

            plbWriter.Write(posX);
            plbWriter.Write(posY);
            plbWriter.Write(posZ);
            plbWriter.Write(ukn1);
            plbWriter.Write(ukn2);

            //Write extra data
            if (extraData != null) foreach (uint word in extraData)
                plbWriter.Write(word);
        }

        public void Build(BinaryWriter binWriter)
        {
            //write item type
            Util.Align(binWriter, 8);
            Util.WriteString(binWriter, type);

            //write item ID
            Util.Align(binWriter, 8);
            Util.WriteString(binWriter, id);

            //write item label
            Util.Align(binWriter, 8);
            Util.WriteString(binWriter, label);
        }
    }

    [DataContract]
    public class MapEntry
    {
        [DataMember(Order = 0)]
        protected string type;
        [DataMember(Order = 1)]
        protected string id;
        [DataMember(Order = 2)]
        protected string label;

        [DataMember(Order = 3)]
        public uint ukn1;
        [DataMember(Order = 3)]
        public uint ukn2;
        [DataMember(Order = 3)]
        public uint ukn3;
        [DataMember(Order = 3)]
        public uint ukn4;

        protected uint[] buildData;

        public string Type
        {
            get { return type; }
            set
            {
                type = value ?? throw new ArgumentNullException("Type");
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                id = value ?? throw new ArgumentNullException("Id");
            }
        }

        public string Label
        {
            get { return label; }
            set
            {
                label = value ?? throw new ArgumentNullException("Label");
            }
        }

        public MapEntry(PLBReader plbReader, ref MapEntryRaw entry, uint headPos)
        {
            type = Util.ReadString(plbReader, entry.typeOfs);
            id = Util.ReadString(plbReader, entry.idOfs);
            label = Util.ReadString(plbReader, entry.labelOfs);

            ukn1 = entry.ukn1;
            ukn2 = entry.ukn2;
            ukn3 = entry.ukn3;
            ukn4 = entry.ukn4;
        }

        public uint BuildPtrs(PLBWriter plbWriter, uint offset, uint header)
        {
            buildData = new uint[3];
            buildData[0] = offset;                                                  //String 1 Addr
            buildData[1] = Util.Align(offset + (uint)(type.Length+1) * 2, 8);       //String 2 Addr
            buildData[2] = Util.Align(buildData[1] + (uint)(id.Length+1) * 2, 8);   //String 3 Addr

            plbWriter.PtrQueue(header + 4);                                         //QUEUE: string 1 pointer pos
            plbWriter.PtrQueue(header + 8);                                         //QUEUE: string 2 pointer pos
            plbWriter.PtrQueue(header + 12);                                        //QUEUE: string 3 pointer pos

            return buildData[2] + (uint)(label.Length+1) * 2;                       //Return end of strings
        }

        public void BuildHeader(BinaryWriter binWriter)
        {
            //write map header
            binWriter.Write(0);
            binWriter.Write(buildData[0]);              //type string ptr
            binWriter.Write(buildData[1]);              //ID string ptr
            binWriter.Write(buildData[2]);              //label string ptr
            binWriter.Write(ukn1);
            binWriter.Write(ukn2);
            binWriter.Write(ukn3);
            binWriter.Write(ukn4);
        }

        public void Build(BinaryWriter binWriter)
        {
            //write item type
            Util.Align(binWriter, 8);
            Util.WriteString(binWriter, type);

            //write item ID
            Util.Align(binWriter, 8);
            Util.WriteString(binWriter, id);

            //write item label
            Util.Align(binWriter, 8);
            Util.WriteString(binWriter, label);
        }

    }

    [DataContract]
    public class Group
    {
        [DataMember(Order = 0)]
        protected string type;
        [DataMember(Order = 1)]
        protected string id;

        [DataMember(Order = 2, EmitDefaultValue = false)]
        protected GroupSection[] sections;
        [DataMember(Order = 3, EmitDefaultValue = false)]
        protected ItemEntry[] items;
        protected ItemEntry[] ukns;        //list of unknown items

        protected uint[] buildData;


        public GroupSection[] Sections { get { return sections; } }
        public ItemEntry[] Items { get { return items; } }
        public ItemEntry[] Ukns { get { return ukns; } }

        public string Type
        {
            get { return type; }
            set
            {
                type = value ?? throw new ArgumentNullException("Type");
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                id = value ?? throw new ArgumentNullException("Id");
            }
        }

        public Group(PLBReader plbReader, ref GroupHeaderRaw obj, uint headPos)
        {
            type = Util.ReadString(plbReader, obj.typeOfs);
            id = Util.ReadString(plbReader, obj.idOfs);

            //Read Sections (Note: group sections are slightly different from regular sections)
            if (obj.sectionListSize > 0)
                Util.LoadListEntries<GroupSectionHeaderRaw, GroupSection>(plbReader, obj.sectionListOfs, obj.sectionListSize, ref sections);

            //Read Items
            if (obj.itemListSize > 0)
                Util.LoadListEntries<ItemEntryRaw, ItemEntry>(plbReader, obj.itemListOfs, obj.itemListSize, ref items);

            //TODO: Load Ukn list?
            if (obj.uknListSize > 0)
                MessagePrinter.AddMsg($"Warning: Ignoring group unknown list at 0x{obj.uknListOfs:X}");
        }

        public uint BuildPtrs(PLBWriter plbWriter, uint offset, uint header)
        {
            buildData = new uint[5];
            buildData[0] = offset;                                                  //String 1 Addr
            buildData[1] = Util.Align(offset + (uint)(type.Length+1) * 2, 8);       //String 2 Addr
            buildData[2] = Util.Align(buildData[1] + (uint)(id.Length + 1) * 2, 4); //group section list Addr

            plbWriter.PtrQueue(header);       //QUEUE: string 1 pointer pos
            plbWriter.PtrQueue(header + 4);   //QUEUE: string 2 pointer pos

            plbWriter.GroupAdd(id, header);

            //Build pointers for group sections
            uint tempOffs = buildData[2];
            uint tempHead = 0;
            if (sections != null)
            {
                tempOffs += (uint)sections.Length * (Util.sizeGroupSectionHeader + 4);  //start of first section
                tempHead = buildData[2] + (uint)sections.Length * 4;

                for (uint i = 0; i < sections.Length; i++)
                {
                    tempOffs = sections[i].BuildPtrs(plbWriter, Util.Align(tempOffs, 8), tempHead);
                    plbWriter.PtrQueue(buildData[2] + i * 4);     //QUEUE: pos of pointer to this section header in section list
                    tempHead += Util.sizeGroupSectionHeader;
                }
            }
            plbWriter.PtrQueue(header + 12);  //QUEUE: GroupSection list pointer pos

            buildData[3] = Util.Align(tempOffs, 4);                             //Item list Addr

            //Build pointers for items
            tempOffs = buildData[3];          
            if (items != null) 
            {
                tempOffs += (uint)items.Length * (Util.sizeItemEntry + 4);      //end of item headers 
                tempHead = buildData[3] + (uint)items.Length * 4;
                foreach (ItemEntry item in items)                               //add space for extra data based on item type (it's in the headers)
                    tempOffs += item.GetExtraBytes();
                for (uint i = 0; i < items.Length; i++)                         //build pointers and get start addr of next item 
                {
                    tempOffs = items[i].BuildPtrs(plbWriter, Util.Align(tempOffs, 8), tempHead);
                    plbWriter.PtrQueue(buildData[3] + i * 4);     //QUEUE: pos of pointer to this in item list
                    tempHead += Util.sizeItemEntry + items[i].GetExtraBytes();
                }
            }
            plbWriter.PtrQueue(header + 20);  //QUEUE: item list pointer pos

            buildData[4] = Util.Align(tempOffs, 4);                             //Ukn list Addr

            //TODO: Build pointers for Ukn's
            plbWriter.PtrQueue(header + 28);  //QUEUE: Ukn list pointer pos

            return tempOffs;
        }

        public void BuildHeader(BinaryWriter binWriter)
        {
            //write group header
            binWriter.Write(buildData[0]);          //type string ptr
            binWriter.Write(buildData[1]);          //ID string ptr
            binWriter.Write(sections?.Length ?? 0); //Section List Size
            binWriter.Write(buildData[2]);          //Section List Offset
            binWriter.Write(items?.Length ?? 0);    //Item List Size
            binWriter.Write(buildData[3]);          //Item List Offset
            binWriter.Write(ukns?.Length ?? 0);     //Ukn List Size;
            binWriter.Write(buildData[4]);          //Ukn List Offset;
            binWriter.Write(0);                     //pad
        }

        public void Build(PLBWriter plbWriter)
        {
            //write pad to 8B
            Util.Align(plbWriter, 8);

            //write group type + pad to 8B
            Util.WriteString(plbWriter, type);
            Util.Align(plbWriter, 8);

            //write group ID + pad to 4B
            Util.WriteString(plbWriter, id);
            Util.Align(plbWriter, 4);

            //write section data
            if (sections != null)
            {
                //write section header list   
                Util.WriteList(plbWriter, buildData[2], sections.Length, Util.sizeGroupSectionHeader);

                //write section headers
                foreach (GroupSection section in sections)
                    section.BuildHeader(plbWriter);

                //build each section
                foreach (GroupSection section in sections)
                    section.Build(plbWriter);
            }

            //Write item Data
            if (items != null)
            {
                //write itemList
                uint temp = buildData[3] + (uint)items.Length * 4;
                plbWriter.Write(temp);
                for (uint i = 1; i < items.Length; i++)
                {
                    temp += items[i - 1].GetExtraBytes();
                    plbWriter.Write(temp + i * 40);
                }

                //write items headers
                foreach (ItemEntry item in items)
                    item.BuildHeader(plbWriter);

                //write item strings
                foreach (ItemEntry item in items)
                    item.Build(plbWriter);
            }

            //write pad to 4B
            Util.Align(plbWriter, 4);

            //TODO: write Ukn data?
        }
    }

    [DataContract]
    public class GroupSection
    {
        [DataMember(Order = 0)]
        protected string type;
        [DataMember(Order = 1)]
        protected string id;

        [DataMember(Order = 2, EmitDefaultValue = false)]
        protected Group[] groups;
        [DataMember(Order = 3, EmitDefaultValue = false)]
        protected ItemEntry[] items;
        [DataMember(Order = 4, EmitDefaultValue = false)]
        protected MapEntry[] maps;

        protected uint[] buildData;


        public Group[] Groups { get { return groups; } }
        public ItemEntry[] Items { get { return items; } }

        public string Type
        {
            get { return type; }
            set
            {
                type = value ?? throw new ArgumentNullException("Type");
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                id = value ?? throw new ArgumentNullException("Id");
            }
        }


        public GroupSection(PLBReader plbReader, ref GroupSectionHeaderRaw section, uint headPos)
        {

            //Read Section Name
            type = Util.ReadString(plbReader, section.stringTypeOfs);
            id  = Util.ReadString(plbReader, section.stringIdOfs);

            //Read Group Ref List
            if (section.groupListSize > 0)
                Util.LoadListEntries<GroupHeaderRaw, Group>(plbReader, section.groupListOfs, section.groupListSize, ref groups);

            //Read Item List
            if (section.itemListSize > 0)
                Util.LoadListEntries<ItemEntryRaw, ItemEntry>(plbReader, section.itemListOfs, section.itemListSize, ref items);

            //Read Map List
            if (section.mapListSize > 0)
                Util.LoadListEntries<MapEntryRaw, MapEntry>(plbReader, section.mapListOfs, section.mapListSize, ref maps);
        }

        //Given start of section, caclulate pointers to contents and return end of section
        public uint BuildPtrs(PLBWriter plbWriter, uint offset, uint header)
        {
            buildData = new uint[5];
            buildData[0] = offset;                                                  //Starting position of this section (Position of type string)
            buildData[1] = Util.Align(offset + (uint)(type.Length + 1) * 2, 8);     //Position of ID string
            buildData[2] = Util.Align(buildData[1] + (uint)(id.Length + 1) * 2, 4); //Position of group list

            plbWriter.PtrQueue(header);       //QUEUE: string 1 pointer pos
            plbWriter.PtrQueue(header + 4);   //QUEUE: string 2 pointer pos

            plbWriter.GroupAdd(id, header);

            //Build Group pointers
            uint tempOffs = buildData[2];
            uint tempHead = 0;
            if (groups != null)
            {
                //start of first group
                tempOffs += (uint)groups.Length * (Util.sizeGroupHeader + 4);
                tempHead = buildData[2] + (uint)groups.Length * 4;

                for (uint i = 0; i < groups.Length; i++)
                {
                    tempOffs = groups[i].BuildPtrs(plbWriter, Util.Align(tempOffs, 8), tempHead);
                    plbWriter.PtrQueue(buildData[2] + i * 4);     //QUEUE: pos of pointer to this Group in Group list
                    tempHead += Util.sizeGroupHeader;
                }
            }
            plbWriter.PtrQueue(header + 12);     //QUEUE: pos of group list pointer in section header

            buildData[3] = Util.Align(tempOffs, 4);                             //Item list Addr

            //Build item pointers
            tempOffs = buildData[3];
            if (items != null)
            {
                tempOffs += (uint)items.Length * (Util.sizeItemEntry + 4);      //end of item headers 
                tempHead = buildData[3] + (uint)items.Length * 4;               //start of first item header
                foreach (ItemEntry item in items)                               //add space for extra data based on item type (it's stored in the headers)
                    tempOffs += item.GetExtraBytes();
                for (uint i = 0; i < items.Length; i++)                         //build pointers and get start addr of next item 
                {
                    tempOffs = items[i].BuildPtrs(plbWriter, Util.Align(tempOffs, 8), tempHead);
                    plbWriter.PtrQueue(buildData[3] + i * 4);     //QUEUE: pos of pointer to this in item list
                    tempHead += Util.sizeItemEntry + items[i].GetExtraBytes();
                }
            }
            plbWriter.PtrQueue(header + 20);     //QUEUE: pos of item list pointer in section header

            buildData[4] = Util.Align(tempOffs, 4);                             //Position of map list

            //Build string pointers for maps
            tempOffs = buildData[4];
            if (maps != null)
            {
                //tempOffs += (uint)maps.Length * (Util.sizeMapEntry + 4);       //end of map headers
                //foreach (MapEntry map in maps)                                 //build pointers and get start addr of next map 
                //    tempOffs = map.BuildPtrs(ptrs, Util.Align(tempOffs, 8), 0);    //TODO: header pos

                tempOffs += (uint)maps.Length * (Util.sizeMapEntry + 4);       //end of map headers
                tempHead = buildData[4] + (uint)maps.Length * 4;
                for (uint i = 0; i < maps.Length; i++)                         //build pointers and get start addr of next map 
                {
                    tempOffs = maps[i].BuildPtrs(plbWriter, Util.Align(tempOffs, 8), tempHead);
                    plbWriter.PtrQueue(buildData[4] + i * 4);     //QUEUE: pos of pointer to this in map list
                    tempHead += Util.sizeMapEntry;
                }
            }
            plbWriter.PtrQueue(header + 28);     //QUEUE: pos of map list pointer in section header

            return tempOffs;
        }

        //Write this section's header
        public void BuildHeader(BinaryWriter binWriter)
        {
            binWriter.Write(buildData[0]);          //type string ptr
            binWriter.Write(buildData[1]);          //ID string ptr
            binWriter.Write(groups?.Length ?? 0);   //Group List Size
            binWriter.Write(buildData[2]);          //Group List Offset
            binWriter.Write(items?.Length ?? 0);    //Item List Size
            binWriter.Write(buildData[3]);          //Item List Offset
            binWriter.Write(maps?.Length ?? 0);     //Map List Size;
            binWriter.Write(buildData[4]);          //Map List Offset;
            binWriter.Write(0);                     //pad
        }

        //Write this section's contents
        public void Build(PLBWriter plbWriter)
        {
            //write pad to 8B
            Util.Align(plbWriter, 8);

            //write section type + pad to 8B
            Util.WriteString(plbWriter, type);
            Util.Align(plbWriter, 8);

            //write section ID + pad to 4B
            Util.WriteString(plbWriter, id);
            Util.Align(plbWriter, 4);

            //Write Group data
            if (groups != null)
            {
                //write section header list (simple ptr calc)   
                Util.WriteList(plbWriter, buildData[2], groups.Length, Util.sizeGroupHeader);

                //write Group headers
                foreach (Group group in groups)
                    group.BuildHeader(plbWriter);

                //build each group
                foreach (Group group in groups)
                    group.Build(plbWriter);
            }

            //Write item Data
            if (items != null)
            {
                //write itemList
                uint temp = buildData[3] + (uint)items.Length * 4;
                plbWriter.Write(temp);
                for (uint i = 1; i < items.Length; i++)
                {
                    temp += items[i - 1].GetExtraBytes();
                    plbWriter.Write(temp + i * 40);
                }

                //write items headers
                foreach (ItemEntry item in items)
                    item.BuildHeader(plbWriter);

                //write item strings
                foreach (ItemEntry item in items)
                    item.Build(plbWriter);
            }

            //write pad to 4B
            Util.Align(plbWriter, 4);

            //Write map Data
            if (maps != null)
            {
                //write map List
                Util.WriteList(plbWriter, buildData[4], maps.Length, Util.sizeMapEntry);

                //write map headers
                foreach (MapEntry map in maps)
                    map.BuildHeader(plbWriter);

                //write map strings
                foreach (MapEntry map in maps)
                    map.Build(plbWriter);
            }

            //write pad to 4B
            Util.Align(plbWriter, 4);
        }
    }

    [DataContract]
    public class Layer
    {
        [DataMember(Order = 0)]
        protected string name;

        [DataMember(Order = 1)]
        public uint data1;
        [DataMember(Order = 1)]
        public uint data2;
        [DataMember(Order = 1)]
        public uint data3;
        [DataMember(Order = 1)]
        public uint data4;

        [DataMember(Order = 2)]
        public uint ukn1;
        [DataMember(Order = 2)]
        public uint ukn2;

        protected uint buildData;

        public string Name
        {
            get { return name; }
            set
            {
                name = value ?? throw new ArgumentNullException("Name");
            }
        }

        public Layer(PLBReader plbReader, ref LayerHeaderRaw layer, uint headPos)
        {
            plbReader.AddLayerAddr(headPos);

            name = Util.ReadString(plbReader, layer.nameOfs);

            data1 = layer.data1;
            data2 = layer.data2;
            data3 = layer.data3;
            data4 = layer.data4;

            ukn1 = layer.ukn1;
            ukn2 = layer.ukn2;
        }

        public uint BuildPtrs(PLBWriter plbWriter, uint offset, uint header)
        {
            buildData = offset;
            plbWriter.PtrQueue(header+4);                   //QUEUE: string 1 pointer pos
            return buildData + (uint)(name.Length+1) * 2;   //Return end of string
        }

        public void BuildHeader(BinaryWriter binWriter)
        {
            //write layer header
            binWriter.Write(0);
            binWriter.Write(buildData);         //name string ptr
            binWriter.Write(data1);
            binWriter.Write(data2);
            binWriter.Write(data3);
            binWriter.Write(data4);
            binWriter.Write(ukn1);
            binWriter.Write(ukn2);
        }

        public void Build(BinaryWriter binWriter)
        {
            //write layer name
            Util.Align(binWriter, 8);
            Util.WriteString(binWriter, name);
        }
    }

    public class Util
    {
        public static readonly uint sizePLBHeader = (uint)Marshal.SizeOf(typeof(PLBHeaderRaw));

        public static readonly uint sizeSectionHeader = (uint)Marshal.SizeOf(typeof(SectionHeaderRaw));
        public static readonly uint sizeGroupRef = (uint)Marshal.SizeOf(typeof(GroupRefRaw));
        public static readonly uint sizeItemEntry = (uint)Marshal.SizeOf(typeof(ItemEntryRaw));
        public static readonly uint sizeMapEntry = (uint)Marshal.SizeOf(typeof(MapEntryRaw));

        public static readonly uint sizeGroupHeader = (uint)Marshal.SizeOf(typeof(GroupHeaderRaw));
        public static readonly uint sizeGroupSectionHeader = (uint)Marshal.SizeOf(typeof(GroupSectionHeaderRaw));

        public static readonly uint sizeLayerHeader = (uint)Marshal.SizeOf(typeof(LayerHeaderRaw));

        public static string ReadString(BinaryReader binReader, uint offset)
        {
            if (offset % 8 > 0)
                MessagePrinter.AddMsg($"Warning: reading misaligned string from 0x{offset:X8}");
            binReader.BaseStream.Position = offset;
            StringBuilder str = new StringBuilder();
            do  //read uint16's as chars until reaching a \0
            {
                str.Append((char)binReader.ReadUInt16());
            } while (str[str.Length - 1] != '\0');
            str.Length--; //remove the \0
            return str.ToString();
        }

        public static void WriteString(BinaryWriter binWriter, string str)
        {
            foreach (char ch in str)
                binWriter.Write((short)ch);
            binWriter.Write((short)0);
        }

        public static int Align(int value, uint alignment)
        {
            return (int) ((value + alignment - 1) / alignment * alignment);
        }

        public static uint Align(uint value, uint alignment)
        {
            return (value + alignment - 1) / alignment * alignment;
        }

        public static void Align(BinaryWriter binWriter, uint alignment)
        {
            while ((binWriter.BaseStream.Position % alignment) != 0)
                binWriter.Write((byte)0);
        }

        public static void ReadList(BinaryReader binReader, List<uint> list, uint listOffset, uint listSize)
        {
            binReader.BaseStream.Position = listOffset;
            while (listSize-- > 0)
                list.Add(binReader.ReadUInt32());
        }

        public static void LoadListEntries<T,V>(BinaryReader binReader, uint listOffset, uint listSize, ref V[] destination)
        {
            GCHandle handle;
            int readSize;
            byte[] readBuffer;

            uint prevOfs = 0;

            destination = new V[listSize];
            T item;
            for (uint i = 0; i < listSize; i++)
            {
                binReader.BaseStream.Position = listOffset + i * 4;
                binReader.BaseStream.Position = binReader.ReadUInt32();

                //Alert if space between structs (but not if its an item's extra data)
                if (i>0)
                {
                    uint space = (uint)(binReader.BaseStream.Position - prevOfs - Marshal.SizeOf(typeof(T)));
                    if (space > 0 && !(typeof(V) == typeof(ItemEntry) && (destination[i - 1] as ItemEntry).GetExtraBytes() == space))
                        MessagePrinter.AddMsg($"Warning: {binReader.BaseStream.Position - prevOfs - Marshal.SizeOf(typeof(T))} bytes of unread space before 0x{binReader.BaseStream.Position:X8}.");
                    //TODO: print out skipped data?
                }

                prevOfs = (uint)binReader.BaseStream.Position;

                readSize = Marshal.SizeOf(typeof(T));
                readBuffer = new byte[readSize];
                readBuffer = binReader.ReadBytes(readSize);
                handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
                item = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
                handle.Free();
                destination[i] = (V)Activator.CreateInstance(typeof(V), new Object[] { binReader, item, prevOfs});
            }
        }

        public static void WriteList(BinaryWriter binWriter, uint startPos, int itemCount, uint itemSize)
        {
            uint temp = startPos + (uint)itemCount * 4;
            for (uint i = 0; i < itemCount; i++)
                binWriter.Write(temp + i * itemSize);
        }
    }

    public class PLBReader : BinaryReader
    {
        private List<uint> layerList = new List<uint>();   //layer list
        private List<uint> groupList = new List<uint>();   //group list
        private Dictionary<string,uint> groups = new Dictionary<string, uint>();

        public PLBReader(Stream input) : base(input) {}
        public PLBReader(Stream input, Encoding encoding) : base(input, encoding) {}
        public PLBReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) {}

        public void AddLayerAddr(uint addr)
        {
            layerList.Add(addr);
        }

        public int FindLayerAddr(uint addr)
        {
            if (addr == 0)
                return -1;
            else
                return layerList.BinarySearch(addr);
        }

        public void AddGroupAddr(uint addr)
        {
            groupList.Add(addr);
        }

        public int FindGroupAddr(uint addr)
        {
            if (addr == 0)
                return -1;
            else
                return groupList.BinarySearch(addr);
        }

        public string GroupUnique(string name, uint ofs)
        {
            int i = 1;
            string nameUnique = name;

            while (groups.ContainsKey(nameUnique))
            {
                if (groups[nameUnique] == ofs)
                    return nameUnique;

                nameUnique = name + "#" + (i++);
            }
            groups.Add(nameUnique, ofs);
            return nameUnique;
        }
    }

    public class PLBWriter : BinaryWriter
    {
        public uint layerHeadAddr;
        private Queue<uint> ptrs = new Queue<uint>();
        private Dictionary<string, uint> groups = new Dictionary<string, uint>();

        public PLBWriter(Stream output) : base(output) {}
        public PLBWriter(Stream output, Encoding encoding) : base(output, encoding) {}
        public PLBWriter(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen) {}

        public void PtrQueue(uint ptr)
        {
            ptrs.Enqueue(ptr);
        }

        public void PtrWrite()
        {
            while (ptrs.Count > 0)
                Write(ptrs.Dequeue());
        }

        public int PtrCount() { return ptrs.Count; }

        public void GroupAdd(string name, uint ptr)
        {
            int i = 1;
            string nameUnique = name;

            while (groups.ContainsKey(nameUnique))
                nameUnique = name + "#" + (i++);

            groups.Add(nameUnique, ptr);
        }
        public uint GroupGetPtr(string name)
        {
            try
            {
                return groups[name];
            }
            catch(Exception e)
            {
                MessagePrinter.AddMsg($"Warning: group {name} does not exist");
                return 0;
            }
        }
    }
}
