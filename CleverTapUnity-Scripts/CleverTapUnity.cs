﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using CleverTap;
using CleverTap.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CleverTapUnity: MonoBehaviour {

    public String CLEVERTAP_ACCOUNT_ID = "TEST-RZZ-8K5-676Z";
    public String CLEVERTAP_ACCOUNT_TOKEN = "TEST-53b-112";
    public String CLEVERTAP_ACCOUNT_REGION = "eu1";
    public int CLEVERTAP_DEBUG_LEVEL = 3;
    public bool CLEVERTAP_ENABLE_PERSONALIZATION = true;
    public bool CLEVERTAP_DISABLE_IDFV;

    void Awake(){
        #if (UNITY_IPHONE && !UNITY_EDITOR)
        DontDestroyOnLoad(gameObject);
        CleverTapBinding.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);
        CleverTapBinding.LaunchWithCredentialsForRegion(CLEVERTAP_ACCOUNT_ID, CLEVERTAP_ACCOUNT_TOKEN, CLEVERTAP_ACCOUNT_REGION);
        if (CLEVERTAP_ENABLE_PERSONALIZATION) {
            CleverTapBinding.EnablePersonalization();
        }
        #endif

        #if (UNITY_ANDROID && !UNITY_EDITOR)
        DontDestroyOnLoad(gameObject);
        CleverTapBinding.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);
        CleverTapBinding.Initialize(CLEVERTAP_ACCOUNT_ID, CLEVERTAP_ACCOUNT_TOKEN, CLEVERTAP_ACCOUNT_REGION);

        Dictionary<string, string> newProps = new Dictionary<string, string>();
            newProps.Add("email", "hp@test.com");
            newProps.Add("Identity", "326");
        CleverTapBinding.OnUserLogin(newProps);
        CleverTapBinding.CreateNotificationChannelWithSound("abtest", "abtest", "CT test channel", 5, true,"test.mp3");
        CleverTapBinding.RecordEvent("unityfinal");
        //==========[Testing Newly added Clevertap APIs]============================================
        //    CleverTapBinding.GetCleverTapId();
        //    CleverTapBinding.ProfileIncrementValueForKey("add_int",2);
        //    CleverTapBinding.ProfileIncrementValueForKey("add_double",3.5);
        //    CleverTapBinding.ProfileDecrementValueForKey("minus_int",2);
        //    CleverTapBinding.ProfileDecrementValueForKey("minus_double",3.5);
        //    CleverTapBinding.SuspendInAppNotifications();
        //    CleverTapBinding.DiscardInAppNotifications();
        //    CleverTapBinding.ResumeInAppNotifications();
        //    CleverTapBinding.RecordEvent("Send Basic Push");
        
        //    CleverTapBinding.RecordEvent("Send Carousel Push");
        //    CleverTapBinding.RecordEvent("Send Manual Carousel Push");
        //    CleverTapBinding.RecordEvent("Send Filmstrip Carousel Push");
        //    CleverTapBinding.RecordEvent("Send Rating Push");
        //    CleverTapBinding.RecordEvent("Send Product Display Notification");
        //    CleverTapBinding.RecordEvent("Send Linear Product Display Push");
        //    CleverTapBinding.RecordEvent("Send CTA Notification");
        //    CleverTapBinding.RecordEvent("Send Zero Bezel Notification");
        //    CleverTapBinding.RecordEvent("Send Zero Bezel Text Only Notification");
        //    CleverTapBinding.RecordEvent("Send Timer Notification");
        //    CleverTapBinding.RecordEvent("Send Input Box Notification");
        //    CleverTapBinding.RecordEvent("Send Input Box Reply with Event Notification");
        //    CleverTapBinding.RecordEvent("Send Input Box Reply with Auto Open Notification");
        //    CleverTapBinding.RecordEvent("Send Input Box Remind Notification DOC FALSE");
        //    CleverTapBinding.RecordEvent("Send Input Box CTA DOC true");
        //    CleverTapBinding.RecordEvent("Send Input Box CTA DOC false");
        //    CleverTapBinding.RecordEvent("Send Input Box Reminder DOC true");
        //    CleverTapBinding.RecordEvent("Send Input Box Reminder DOC false");
        
        //==========[Testing Newly added Clevertap APIs]============================================
        if (CLEVERTAP_ENABLE_PERSONALIZATION) {
            CleverTapBinding.EnablePersonalization();
        }
        #endif
    }
    
    void Start() {}

    // handle deep link url
    void CleverTapDeepLinkCallback(string url) {
        Debug.Log("unity received deep link: " + (!String.IsNullOrEmpty(url) ? url : "NULL"));
    }

    // called when then the CleverTap user profile is initialized
    // returns {"CleverTapID":<CleverTap unique user id>}
    void CleverTapProfileInitializedCallback(string message) {
        Debug.Log("unity received profile initialized: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

        if (String.IsNullOrEmpty(message)) {
            return;
        }

        try {
            JSONClass json = (JSONClass)JSON.Parse(message);
            Debug.Log(String.Format("unity parsed profile initialized {0}", json));
        } catch {
            Debug.LogError("unable to parse json");
        }
    }

    // called when the user profile is updated as a result of a server sync
    /** 
        returns dict in the form:
        {
            "profile":{"<property1>":{"oldValue":<value>, "newValue":<value>}, ...},
            "events:{"<eventName>":
                        {"count":
                            {"oldValue":(int)<old count>, "newValue":<new count>},
                        "firstTime":
                            {"oldValue":(double)<old first time event occurred>, "newValue":<new first time event occurred>},
                        "lastTime":
                            {"oldValue":(double)<old last time event occurred>, "newValue":<new last time event occurred>},
                    }, ...
                }
        }
    */
    void CleverTapProfileUpdatesCallback(string message) {
        Console.WriteLine("Clevertap unity"+message);
        Debug.Log("unity received profile updates: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

        if (String.IsNullOrEmpty(message)) {
            return;
        }

        try {
            JSONClass json = (JSONClass)JSON.Parse(message);
            Debug.Log(String.Format("unity parsed profile updates {0}", json));
        } catch {
            Debug.LogError("unable to parse json");
        }
    }	
		
    // returns the data associated with the push notification
    void CleverTapPushOpenedCallback(string message) {
        Debug.Log("unity received push opened: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

        if (String.IsNullOrEmpty(message)) {
            return;
        }

        try {
            JSONClass json = (JSONClass)JSON.Parse(message);
            Debug.Log(String.Format("push notification data is {0}", json));
        } catch {
            Debug.LogError("unable to parse json");
        }
    }

    // returns a unique CleverTap identifier suitable for use with install attribution providers.
    void CleverTapInitCleverTapIdCallback(string message) {
        Debug.Log("unity received clevertap id: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns the custom data associated with an in-app notification click
    void CleverTapInAppNotificationDismissedCallback(string message) {
        Debug.Log("unity received inapp notification dismissed: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns when an in-app notification is dismissed by a call to action with custom extras
    void CleverTapInAppNotificationButtonTapped(string message) {
        Debug.Log("unity received inapp notification button tapped: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns callback for InitializeInbox
    void CleverTapInboxDidInitializeCallback(){
        Debug.Log("unity received inbox initialized");
    }

    void CleverTapInboxMessagesDidUpdateCallback(){
        Debug.Log("unity received inbox messages updated");
    }

    // returns on the click of app inbox message with a map of custom Key-Value pairs
    void CleverTapInboxCustomExtrasButtonSelect(string message) {
        Debug.Log("unity received inbox message button with custom extras select: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // returns native display units data
    void CleverTapNativeDisplayUnitsUpdated(string message) {
        Debug.Log("unity received native display units updated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Product Config are fetched 
    void CleverTapProductConfigFetched(string message) {
        Debug.Log("unity received product config fetched: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Product Config are activated
    void CleverTapProductConfigActivated(string message) {
        Debug.Log("unity received product config activated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Product Config are initialized
    void CleverTapProductConfigInitialized(string message) {
        Debug.Log("unity received product config initialized: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    // invoked when Product Experiences - Feature Flags are updated 
    void CleverTapFeatureFlagsUpdated(string message) {
        Debug.Log("unity received feature flags updated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
    }

    #if UNITY_EDITOR
    private void OnValidate() {
        EditorPrefs.SetBool("CLEVERTAP_DISABLE_IDFV", CLEVERTAP_DISABLE_IDFV);
    }
    #endif
}
