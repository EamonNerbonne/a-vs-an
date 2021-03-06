﻿using AvsAnLib.Internals;
using WikipediaAvsAnTrieExtractor;
using Xunit;

namespace WikipediaAvsAnTrieExtractorTest {
    public class SerializerTest {
        [Fact]
        public void SingleNodeWorks() {
            var node = new Node { ratio = { aCount = 0x2468ad, anCount = 0x12345 } };
            const string serializedNode = @"(2468ad:12345)";
            Assert.Equal(serializedNode, NodeSerializer.Serialize(node));
            Assert.Equal(node, NodeSerializer.Deserialize(serializedNode), NodeEqualityComparer.Instance);
        }

        [Fact]
        public void RootNodeWithKidsWorks() {
            var node = new Node {
                ratio = { aCount = 1, anCount = 11 },
                SortedKids = new[] {
                    new Node {
                        c = 'b',
                        ratio = { aCount = 5, anCount = 0 },
                    },
                    new Node {
                        c = 'u',
                        ratio = { aCount = 2, anCount = 15 },
                    },
                }
            };

            const string serializedNode = @"(1:b)b(5:0)u(2:f)";
            Assert.Equal(serializedNode, NodeSerializer.Serialize(node));
            var deserialized = NodeSerializer.Deserialize(serializedNode);
            Assert.Equal(node, deserialized, NodeEqualityComparer.Instance);
        }

        [Fact]
        public void SerializeFourLevelTree() {
            var node = new Node {
                ratio = { aCount = 1, anCount = 11 },
                SortedKids = new[] {
                    new Node {
                        c = 'b',
                        ratio = { aCount = 5, anCount = 0 },
                        SortedKids = new[] {
                            new Node {
                                c = 'c',
                                ratio = { aCount = 3, anCount = 4 },
                                SortedKids = new[] {
                                    new Node {
                                        c = 'd',
                                        ratio = { aCount = 0x100, anCount = 0x80 }
                                    },
                                },
                            },
                            new Node {
                                c = 'u',
                                ratio = { aCount = 2, anCount = 15 }
                            },
                        },
                    },
                }
            };
            const string serializedNode = @"(1:b)b(5:0)bc(3:4)bcd(100:80)bu(2:f)";

            Assert.Equal(serializedNode, NodeSerializer.Serialize(node));
            Assert.Equal(node, NodeSerializer.Deserialize(serializedNode), NodeEqualityComparer.Instance);
        }
    }
}
