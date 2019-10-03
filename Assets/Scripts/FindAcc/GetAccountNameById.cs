using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace Assets.Accounts.Hashtag.GetAccountName
{
    [DataContract]
    public class HdProfilePicVersion
    {
        public int width { get; set; }
        public int height { get; set; }
        public string url { get; set; }
    }

    [DataContract]
    public class HdProfilePicUrlInfo
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
    [DataContract]
    public class User
    {
        public long pk { get; set; }
        [DataMember(Name = "username")]
        public string username { get; set; }
        public string full_name { get; set; }
        public bool is_private { get; set; }
        public string profile_pic_url { get; set; }
        public string profile_pic_id { get; set; }
        public bool is_verified { get; set; }
        public bool has_anonymous_profile_picture { get; set; }
        public int media_count { get; set; }
        public int follower_count { get; set; }
        public int following_count { get; set; }
        public int following_tag_count { get; set; }
        public string biography { get; set; }
        public string external_url { get; set; }
        public string external_lynx_url { get; set; }
        public int total_igtv_videos { get; set; }
        public int total_ar_effects { get; set; }
        public int usertags_count { get; set; }
        public bool is_favorite { get; set; }
        public bool is_interest_account { get; set; }
        public List<HdProfilePicVersion> hd_profile_pic_versions { get; set; }
        public HdProfilePicUrlInfo hd_profile_pic_url_info { get; set; }
        public int mutual_followers_count { get; set; }
        public bool has_highlight_reels { get; set; }
        public bool can_be_reported_as_fraud { get; set; }
        public bool is_business { get; set; }
        public int account_type { get; set; }
        public object is_call_to_action_enabled { get; set; }
        public bool include_direct_blacklist_status { get; set; }
        public bool is_potential_business { get; set; }
        public bool is_bestie { get; set; }
        public bool has_unseen_besties_media { get; set; }
        public bool show_account_transparency_details { get; set; }
        public bool auto_expand_chaining { get; set; }
        public bool highlight_reshare_disabled { get; set; }
        public bool show_post_insights_entry_point { get; set; }
        public bool about_your_account_bloks_entrypoint_enabled { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember(Name = "user")]
        public User user { get; set; }
        public string status { get; set; }
    }
}
