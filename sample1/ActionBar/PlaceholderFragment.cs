
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;


namespace ActionBarProto
{


    public class PlaceholderFragment : Fragment
    {
        private static string ARG_SECTION_NUMBER = "section_number";

        public PlaceholderFragment() {
        }

        public static PlaceholderFragment newInstance(int sectionNumber) {
            PlaceholderFragment fragment = new PlaceholderFragment();
            Bundle args = new Bundle();
            args.PutInt(ARG_SECTION_NUMBER, sectionNumber);
            fragment.Arguments = args;
            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
            Bundle savedInstanceState) {
            View rootView = inflater.Inflate(Resource.Layout.fragment_main, container, false);
            TextView textView = rootView.FindViewById<TextView>(Resource.Id.section_label);
            var arg = Arguments.GetInt(ARG_SECTION_NUMBER);
            textView.Text = arg.ToString();
            return rootView;
        }

        public override void OnAttach(Activity activity) {
            base.OnAttach(activity);
            var value = Arguments.GetInt(ARG_SECTION_NUMBER);
            ((MainActivity)activity).OnSectionAttached(value);
        }
    }
}

