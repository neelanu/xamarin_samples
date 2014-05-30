using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


namespace ActionBarProto
{
    [Activity(Label = "ActionBarProto", MainLauncher = true)]
    public class MainActivity : Activity, NavigationDrawerCallbacks
    {
        int count = 1;

        private NavigationDrawerFragment mNavigationDrawerFragment;
        private int mTitle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            mNavigationDrawerFragment = FragmentManager.FindFragmentById<NavigationDrawerFragment>(Resource.Id.navigation_drawer);
            //mTitle = Title

            mNavigationDrawerFragment.setUp(
                Resource.Id.navigation_drawer,
                FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer_layout));
        }

        public void OnNavigationDrawerItemSelected(int position) {
            this.FragmentManager.BeginTransaction()
                .Replace(Resource.Id.container, PlaceholderFragment.newInstance(position + 1))
                .Commit();
        }

        public  void OnSectionAttached(int number) {
            switch (number) {
                case 1:
                    mTitle = Resource.String.title_section1;
                    break;
                case 2:
                    mTitle = Resource.String.title_section2;
                    break;
                case 3:
                    mTitle = Resource.String.title_section3;
                    break;
            }
        }


        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item) {
            // Handle action bar item clicks here. The action bar will
            // automatically handle clicks on the Home/Up button, so long
            // as you specify a parent activity in AndroidManifest.xml.
            int id = item.ItemId;//.getItemId();
            if (id == Resource.Id.action_settings) {
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }


        public override bool OnCreateOptionsMenu(IMenu menu) {
            if (!mNavigationDrawerFragment.isDrawerOpen()) {
                // Only show items in the action bar relevant to this screen
                // if the drawer is not showing. Otherwise, let the drawer
                // decide what to show in the action bar.
                MenuInflater.Inflate(Resource.Menu.main, menu);
                restoreActionBar();
                return true;
            }
            return base.OnCreateOptionsMenu(menu);
        }

        public void restoreActionBar() {
            ActionBar actionBar = this.ActionBar;

            actionBar.NavigationMode = ActionBarNavigationMode.Standard; //SetNavigationMode(ActionBar.NAVIGATION_MODE_STANDARD);
            actionBar.SetDisplayShowTitleEnabled(true);
            actionBar.SetTitle(mTitle);
        }
    }
}


