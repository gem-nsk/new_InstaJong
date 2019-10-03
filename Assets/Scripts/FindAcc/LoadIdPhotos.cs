using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Accounts.LoadImages
{

    public class PageInfo
    {
        public bool has_next_page { get; set; }
        public object end_cursor { get; set; }
    }

    public class Node2
    {
        [DataMember(Name = "text")]
        public string text { get; set; }
    }

    public class Edge2
    {
        [DataMember(Name = "node")]
        public Node2 node { get; set; }
    }

    public class EdgeMediaToCaption
    {
        [DataMember(Name = "edges")]
        public List<Edge2> edges { get; set; }
    }

    public class EdgeMediaToComment
    {
        [DataMember(Name = "count")]
        public int count { get; set; }
    }

    public class Dimensions
    {
        public int height { get; set; }
        public int width { get; set; }
    }

    public class EdgeMediaPreviewLike
    {
        [DataMember(Name = "count")]
        public int count { get; set; }
    }

    public class Owner
    {
        public string id { get; set; }
    }

    public class ThumbnailResource
    {
        public string src { get; set; }
        public int config_width { get; set; }
        public int config_height { get; set; }
    }

    public class Node
    {
        public string id { get; set; }
        public string __typename { get; set; }
        [DataMember(Name = "edge_media_to_caption")]
        public EdgeMediaToCaption edge_media_to_caption { get; set; }
        [DataMember(Name = "shortcode")]
        public string shortcode { get; set; }
        [DataMember(Name = "edge_media_to_comment")]
        public EdgeMediaToComment edge_media_to_comment { get; set; }
        public bool comments_disabled { get; set; }
        public int taken_at_timestamp { get; set; }
        public Dimensions dimensions { get; set; }
        [DataMember(Name = "display_url")]
        public string display_url { get; set; }
        [DataMember(Name = "edge_media_preview_like")]
        public EdgeMediaPreviewLike edge_media_preview_like { get; set; }
        public Owner owner { get; set; }
        [DataMember(Name = "thumbnail_src")]
        public string thumbnail_src { get; set; }
        public List<ThumbnailResource> thumbnail_resources { get; set; }
        public bool is_video { get; set; }
    }

    public class Edge
    {
        [DataMember(Name = "node")]
        public Node node { get; set; }
    }

    public class EdgeOwnerToTimelineMedia
    {
        public int count { get; set; }
        public PageInfo page_info { get; set; }
        [DataMember(Name = "edges")]
        public List<Edge> edges { get; set; }
    }

    public class User
    {
        [DataMember(Name = "edge_owner_to_timeline_media")]
        public EdgeOwnerToTimelineMedia edge_owner_to_timeline_media { get; set; }
    }

    public class Data
    {
        [DataMember(Name = "user")]

        public User user { get; set; }
    }
    [DataContract]
    public class RootObject
    {
        [DataMember(Name = "data")]
        public Data data { get; set; }
        public string status { get; set; }

    }
}