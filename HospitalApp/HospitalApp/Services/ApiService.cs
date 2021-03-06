﻿namespace HospitalApp.Services
{
    using Helpers;
    using Model;
    using Model.Request;
    using Plugin.Connectivity;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ViewModel;

    public class ApiService : HttpService
    {
        #region users
        public async Task<UserResponse> UserProfile(UserRequest user)
        {
            var token = await GetToken(user.Email, user.Password);

            if(token == null || token.AccessToken == null)
            {
                return null;
            }

            //var userResponse = await Post(, user);

            var userResponse = new UserResponse()
            {
                Email = user.Email
            };

            Settings.AccessToken = token.AccessToken;

            return userResponse;

        }
        #endregion

        #region Doctor
        public async Task<List<DoctorResponse>> GetDoctor()
        {
            var response = await Get<DoctorResponse>(FirstVersion, "/Doctors/List/");

            if (!response.IsSuccess) return null;

            return (List<DoctorResponse>)response.Result;
        }
        public async Task<bool> NewDoctor(DoctorRequest doctor)
        {
            var response = await PostStatusCode(FirstVersion, "/Doctors/Create/", doctor);

            return response.IsSuccess;
        }
        #endregion

        #region Specialities
        public async Task<List<SpecialityResponse>> GetSpecialities()
        {
            var response = await Get<SpecialityResponse>(FirstVersion, "/Specialities/List/");

            if (!response.IsSuccess) return null;

            return (List<SpecialityResponse>)response.Result;
        }

        public async Task<string> NewSpeciality(SpecialityRequest speciality)
        {
            var response = await PostStatusCode(FirstVersion, "/Specialities/Create", speciality);

            return response.Message;
        }
        #endregion

        #region Methods
        public async Task<bool> IsConnection()
        {

            if (!CrossConnectivity.Current.IsConnected)
            {
                await MainViewModel.GetInstance().DialogService.ShowMessage("Error", "No dispones de conexión a internet");

                return false;
            }

            if (!await CrossConnectivity.Current.IsRemoteReachable("google.com"))
            {
                await MainViewModel.GetInstance().DialogService.ShowMessage("Error", "No dispones de conexión a internet");

                return false;
            }

            return true;
        }
        #endregion
    }
}
