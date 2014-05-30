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
    public class NavigationDrawerFragment : Fragment
    {
        private string STATE_SELECTED_POSITION = "selected_navigation_drawer_position";
        private string PREF_USER_LEARNED_DRAWER = "navigation_drawer_learned";

        private CustomActionBarDrawerToggle mDrawerToggle;
        private Android.Support.V4.Widget.DrawerLayout mDrawerLayout;

        private ListView mDrawerListView;
        private View mFragmentContainerView;

        private int mCurrentSelectedPosition = 0;
        private bool mFromSavedInstanceState;
        private bool mUserLearnedDrawer;
        private NavigationDrawerCallbacks mCallbacks;

        public NavigationDrawerFragment() {
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ISharedPreferences sp = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            mUserLearnedDrawer = sp.GetBoolean(PREF_USER_LEARNED_DRAWER, false);

            if (savedInstanceState != null) {
                mCurrentSelectedPosition = savedInstanceState.GetInt(STATE_SELECTED_POSITION);
                mFromSavedInstanceState = true;
            }

            selectItem(mCurrentSelectedPosition);

        }

        public override void OnActivityCreated (Bundle savedInstanceState) {
            base.OnActivityCreated(savedInstanceState);
            // Indicate that this fragment would like to influence the set of actions in the action bar.
            SetHasOptionsMenu(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            mDrawerListView = (ListView) inflater.Inflate(Resource.Layout.fragment_navigation_drawer, container, false);
            mDrawerListView.SetItemChecked(mCurrentSelectedPosition, true);
            mDrawerListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => selectItem(e.Position);
            string[] title = new string[]
            {
                GetString(Resource.String.title_section1),
                GetString(Resource.String.title_section2),
                GetString(Resource.String.title_section3),
            };


            ArrayAdapter listAdapter = new ArrayAdapter(Activity, //this.getActionBar().ThemedContext, 
                                           Android.Resource.Layout.SimpleListItemActivated1, Android.Resource.Id.Text1, title);
            mDrawerListView.SetAdapter(listAdapter);
            mDrawerListView.SetItemChecked(mCurrentSelectedPosition, true);
            return mDrawerListView;
        }

        public bool isDrawerOpen() {
            return mDrawerLayout != null && mDrawerLayout.IsDrawerOpen(mFragmentContainerView);
        }

        public void setUp(int fragmentId, Android.Support.V4.Widget.DrawerLayout drawerLayout) {
            mFragmentContainerView = this.Activity.FindViewById<ListView>(fragmentId);
            mDrawerLayout = drawerLayout;
            mDrawerLayout.SetDrawerShadow(Resource.Drawable.drawer_shadow, (int)GravityFlags.Start);

            var actionBar = Activity.ActionBar; //ActionBar actionBar = getActionBar();
            actionBar.SetDisplayHomeAsUpEnabled(true);
            actionBar.SetHomeButtonEnabled(true);

            mDrawerToggle = new CustomActionBarDrawerToggle(
                this.Activity,                    /* host Activity */
                mDrawerLayout,                    /* DrawerLayout object */
                Resource.Drawable.ic_drawer,             /* nav drawer image to replace 'Up' caret */
                Resource.String.navigation_drawer_open,  /* "open drawer" description for accessibility */
                Resource.String.navigation_drawer_close  /* "close drawer" description for accessibility */
            );

            mDrawerToggle.DrawerClosed += delegate
            {
                if (! IsAdded) {
                    return;
                } 
                Activity.InvalidateOptionsMenu();
            };

            mDrawerToggle.DrawerOpened += delegate
            {
                if (! IsAdded) {
                    return;
                }
                if (!mUserLearnedDrawer) {
                    mUserLearnedDrawer = true;
                    ISharedPreferences sp = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                    ISharedPreferencesEditor editor = sp.Edit();
                    editor.PutBoolean(PREF_USER_LEARNED_DRAWER, true).Apply();
                }
                Activity.InvalidateOptionsMenu();
            };

            if (!mUserLearnedDrawer && !mFromSavedInstanceState) {
                mDrawerLayout.OpenDrawer(mFragmentContainerView);
            }

            mDrawerLayout.Post(() =>
            {
                mDrawerToggle.SyncState();
            });

            mDrawerLayout.SetDrawerListener(mDrawerToggle);
        }

        private void selectItem(int position) {
            mCurrentSelectedPosition = position;
            if (mDrawerListView != null) {
                mDrawerListView.SetItemChecked(position, true);
            }
            if (mDrawerLayout != null) {
                mDrawerLayout.CloseDrawer(mFragmentContainerView);
            }
            if (mCallbacks != null) {
                mCallbacks.OnNavigationDrawerItemSelected(position);
            }
        }

        public override void OnAttach(Activity activity) {
            base.OnAttach(activity);
            mCallbacks = (NavigationDrawerCallbacks) activity;
        }

        public override void OnDetach() {
            base.OnDetach();
            mCallbacks = null;
        }

        public override void OnSaveInstanceState(Bundle outState) {
            base.OnSaveInstanceState(outState);
            outState.PutInt(STATE_SELECTED_POSITION, mCurrentSelectedPosition);
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig) {
            base.OnConfigurationChanged(newConfig);
            // Forward the new configuration the drawer toggle component.
            mDrawerToggle.OnConfigurationChanged(newConfig);
        }

        public void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater) {
            // If the drawer is open, show the global app actions in the action bar. See also
            // showGlobalContextActionBar, which controls the top-left area of the action bar.
            if (mDrawerLayout != null && isDrawerOpen()) {
                inflater.Inflate(Resource.Menu.global, menu);
                showGlobalContextActionBar();
            }
            base.OnCreateOptionsMenu(menu, inflater);
        }

        private void showGlobalContextActionBar() {
            Android.App.ActionBar actionBar = Activity.ActionBar;
            actionBar.SetDisplayShowTitleEnabled(true);
            actionBar.NavigationMode = ActionBarNavigationMode.Standard; //SetNavigationMode(Android.App.ActionBar.NAVIGATION_MODE_STANDARD);
            actionBar.SetTitle(Resource.String.app_name);
        }

        public override bool OnOptionsItemSelected(IMenuItem item) {
            if (mDrawerToggle.OnOptionsItemSelected(item)) {
                return true;
            }

            if (item.ItemId == Resource.Id.action_example) {
                //Toast.makeText(getActivity(), "Example action.", Toast.LENGTH_SHORT).show();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
   
    }



}

