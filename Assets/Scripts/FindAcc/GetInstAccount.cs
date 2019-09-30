using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
namespace Assets.Accounts
{
    [DataContract]
    public class EdgeFollowedBy
    {
        public int count { get; set; }
    }

    [DataContract]
    public class EdgeFollow
    {
        public int count { get; set; }
    }

    [DataContract]
    public class EdgeMutualFollowedBy
    {
        public int count { get; set; }
        public List<object> edges { get; set; }
    }

    [DataContract]
    public class PageInfo
    {
        public bool has_next_page { get; set; }
        public object end_cursor { get; set; }
    }

    [DataContract]
    public class EdgeFelixVideoTimeline
    {
        public int count { get; set; }
        public PageInfo page_info { get; set; }
        public List<object> edges { get; set; }
    }

    [DataContract]
    public class PageInfo2
    {
        public bool has_next_page { get; set; }
        public string end_cursor { get; set; }
    }

    [DataContract]
    public class Node2
    {
        [DataMember(Name = "text")]
        public string text { get; set; }
    }

    [DataContract]
    public class Edge2
    {
        [DataMember(Name = "node")]
        public Node2 node { get; set; }
    }

    [DataContract]
    public class EdgeMediaToCaption
    {
        [DataMember(Name = "edges")]
        public List<Edge2> edges { get; set; }
    }

    [DataContract]
    public class EdgeMediaToComment
    {
        [DataMember(Name = "count")]
        public int count { get; set; }
    }

    [DataContract]
    public class Dimensions
    {
        public int height { get; set; }
        public int width { get; set; }
    }

    [DataContract]
    public class EdgeLikedBy
    {
        [DataMember(Name = "count")]
        public int count { get; set; }
    }

    [DataContract]
    public class EdgeMediaPreviewLike
    {
        public int count { get; set; }
    }

    [DataContract]
    public class Location
    {
        public string id { get; set; }
        public bool has_public_page { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    [DataContract]
    public class Owner
    {
        public string id { get; set; }
        public string username { get; set; }
    }

    [DataContract]
    public class ThumbnailResource
    {
        public string src { get; set; }
        public int config_width { get; set; }
        public int config_height { get; set; }
    }

    [DataContract]
    public class Node
    {
        public string __typename { get; set; }
        public string id { get; set; }
        [DataMember(Name = "edge_media_to_caption")]
        public EdgeMediaToCaption edge_media_to_caption { get; set; }
        public string shortcode { get; set; }
        [DataMember(Name = "edge_media_to_comment")]
        public EdgeMediaToComment edge_media_to_comment { get; set; }
        public bool comments_disabled { get; set; }
        public int taken_at_timestamp { get; set; }
        public Dimensions dimensions { get; set; }
        [DataMember(Name = "display_url")]
        public string display_url { get; set; }
        [DataMember(Name = "edge_liked_by")]
        public EdgeLikedBy edge_liked_by { get; set; }
        public EdgeMediaPreviewLike edge_media_preview_like { get; set; }
        public Location location { get; set; }
        public object gating_info { get; set; }
        public object fact_check_information { get; set; }
        public string media_preview { get; set; }
        public Owner owner { get; set; }
        [DataMember(Name = "thumbnail_src")]
        public string thumbnail_src { get; set; }
        public List<ThumbnailResource> thumbnail_resources { get; set; }
        public bool is_video { get; set; }
        public string accessibility_caption { get; set; }
    }

    [DataContract]
    public class Edge
    {
        [DataMember(Name = "node")]
        public Node node { get; set; }
    }

    [DataContract]
    public class EdgeOwnerToTimelineMedia
    {
        public int count { get; set; }
        public PageInfo2 page_info { get; set; }
        [DataMember(Name = "edges")]
        public List<Edge> edges { get; set; }
    }

    [DataContract]
    public class PageInfo3
    {
        public bool has_next_page { get; set; }
        public object end_cursor { get; set; }
    }

    [DataContract]
    public class EdgeSavedMedia
    {
        public int count { get; set; }
        public PageInfo3 page_info { get; set; }
        public List<object> edges { get; set; }
    }
    [DataContract]

    public class PageInfo4
    {
        public bool has_next_page { get; set; }
        public object end_cursor { get; set; }
    }
    [DataContract]

    public class EdgeMediaCollections
    {
        public int count { get; set; }
        public PageInfo4 page_info { get; set; }
        public List<object> edges { get; set; }
    }

    [DataContract]
    public class User
    {
        public string biography { get; set; }
        public bool blocked_by_viewer { get; set; }
        public bool country_block { get; set; }
        public object external_url { get; set; }
        public object external_url_linkshimmed { get; set; }
        public EdgeFollowedBy edge_followed_by { get; set; }
        public bool followed_by_viewer { get; set; }
        public EdgeFollow edge_follow { get; set; }
        public bool follows_viewer { get; set; }
        public string full_name { get; set; }
        public bool has_channel { get; set; }
        public bool has_blocked_viewer { get; set; }
        public int highlight_reel_count { get; set; }
        public bool has_requested_viewer { get; set; }
        [DataMember(Name = "id")]
        public string id { get; set; }
        public bool is_business_account { get; set; }
        public bool is_joined_recently { get; set; }
        public object business_category_name { get; set; }
        public bool is_private { get; set; }
        public bool is_verified { get; set; }
        public EdgeMutualFollowedBy edge_mutual_followed_by { get; set; }
        public string profile_pic_url { get; set; }
        public string profile_pic_url_hd { get; set; }
        public bool requested_by_viewer { get; set; }
        [DataMember(Name = "username")]
        public string username { get; set; }
        public object connected_fb_page { get; set; }
        public EdgeFelixVideoTimeline edge_felix_video_timeline { get; set; }
        [DataMember(Name = "edge_owner_to_timeline_media")]
        public EdgeOwnerToTimelineMedia edge_owner_to_timeline_media { get; set; }
        public EdgeSavedMedia edge_saved_media { get; set; }
        public EdgeMediaCollections edge_media_collections { get; set; }
    }
    [DataContract]
    public class Graphql
    {
        [DataMember(Name = "user")]
        public User user { get; set; }
    }
    [DataContract]
    public class RootObject
    {
        public string logging_page_id { get; set; }
        public bool show_suggested_profiles { get; set; }
        public bool show_follow_dialog { get; set; }
        [DataMember(Name = "graphql")]
        public Graphql graphql { get; set; }
        public object toast_content_on_load { get; set; }
    }
}