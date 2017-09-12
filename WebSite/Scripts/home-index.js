angular.module('app', ['ngAnimate', 'ngSanitize', 'ui.select', 'ui.bootstrap'])
  .controller('Controller', function ($http, $scope) {
      var ctrl = this;

      //PetCategoryOptions
      ctrl.getAllPetCategoryOptions = function () {
          $http.get('/home/GetAllPetCategoryOptions')
    .then(function (response) {
        ctrl.petCategoryOptions = response.data.Result;
    });
      };
      ctrl.getAllPetCategoryOptions();


      //PetSize
      ctrl.getAllPetSizeOptions = function () {
          $http.get('/home/GetAllPetSizeOptions')
          .then(function (response) {
              ctrl.petSizeOptions = response.data.Result;
          });
      };
      ctrl.getAllPetSizeOptions();
   

      //PetCareOption
      ctrl.getAllPetCareOptionOptions = function () {
          $http.get('/home/GetAllPetCareOptionOptions')
          .then(function (response) {
              ctrl.petCareOption = response.data.Result;
          });
      };
      ctrl.getAllPetCareOptionOptions();


      //Address
      //ctrl.address is an object {}, [] is an Array
      ctrl.address = {};  
      ctrl.refreshAddresses = function (searchText) {
          if (searchText && searchText.length >= 2) {
              var searchRequest = { address: searchText, sensor: false };
              return $http.get('http://maps.googleapis.com/maps/api/geocode/json', { params: searchRequest })
                  .then(function (response) {
                  ctrl.addresses = response.data.results;
                  }).catch(function () { alert("error fetching addresses"); }
                  );
          }       
      };



      //Check-in & Check-out Date
      ctrl.fromDt = null;
      ctrl.toDt = null;

      //if toDt(check-out) is selected first, then the maxDate option for fromDt(check-in) is toDt(check-out)
      $scope.$watch("ctrl.toDt", function () {
          if (ctrl.toDt && ! ctrl.fromDt) {
              ctrl.fromDt = ctrl.toDt;
              ctrl.fromDateOptions.maxDate = ctrl.toDt;
          }
      });

      // if the fromDt (check-in) is assigned first, then the minDate for toDate(check-out) option will be the fromDt(check-in).
      $scope.$watch("ctrl.fromDt", function () {
          if (ctrl.fromDt && ! ctrl.toDt) {
              ctrl.toDt = ctrl.fromDt;
              ctrl.toDateOptions.minDate = ctrl.fromDt;
          }
      });

      // Set the maxDate to 90 days later from current date
      ctrl.getMaxDate = function () {
          var maxDt = new Date();
          return maxDt.setDate(maxDt.getDate() + 90);
      };

      //Set the minDate: 
      //if toDt(check-out) is not assigned yet, then the minDate is the current date.
      // if toDt(cehck-out) is assigned first, which means the user might select the check-out date first, then the minDate for fromDt(check-in) is the current date.
      ctrl.getMinDate = function () {
          if (!ctrl.toDt)
              return new Date();
          //else
          //    ctrl.toDt;
      };

      ctrl.fromDateOptions = {
          //disabledDate: ctrl.disabledDate(),
          showWeeks: false,
          minDate: ctrl.getMinDate(),
          maxDate: ctrl.getMaxDate()
      };

      ctrl.toDateOptions = {
          //disabledDate: ctrl.disabledDate(),
          showWeeks: false,
          minDate: ctrl.getMinDate(),
          maxDate: ctrl.getMaxDate()
      };

      ctrl.fromDatePickerPopup = {
          opened: false
      };

      ctrl.toDatePickerPopup = {
          opened: false
      };
      ctrl.openFromDatePicker = function () {
          ctrl.fromDatePickerPopup.opened = true;
      }; 

      ctrl.openToDatePicker = function () {
          ctrl.toDatePickerPopup.opened = true;
      };



      //If PetCategory "Cat"  or "Other" is selected, Set selectedPetSize = 'S'
      $scope.$watch("ctrl.selectedPetCategory", function () {
          if (ctrl.selectedPetCategory.PetCategoryName === 'Cat' || ctrl.selectedPetCategory.PetCategoryName === 'Other') {
              ctrl.selectedPetSize = { PetSizeName: "Small", PetSizeId: 'S' };
          }
      })


      // Validate user input and display the result
      ctrl.petsitters = [];
      ctrl.zip = "";
      ctrl.city = "";
      ctrl.state = "";

      ctrl.submitForm = function (isValid) {
          ctrl.submitted = true;
          if (isValid) {

              // all required input is filled
              ctrl.valid = true;

              //Assign location attributes
                  //for (i = 0; i < ctrl.selectedAddress.address_components.length; i++) {
                      var addressType = ctrl.selectedAddress.address_components[0].types[0];

                      if (addressType === "postal_code") {
                          ctrl.zip = ctrl.selectedAddress.address_components[0].short_name;
                          ctrl.city = ctrl.selectedAddress.address_components[1].short_name;
                          ctrl.state = ctrl.selectedAddress.address_components[3].short_name;
                      }
                      else if (addressType === "locality") {
                          ctrl.city = ctrl.selectedAddress.address_components[0].short_name;
                          ctrl.state = ctrl.selectedAddress.address_components[2].short_name;
                      }
                      else if (addressType === "administrative_area_level_1") {
                          ctrl.state = ctrl.selectedAddress.address_components[0].short_name;
                      }
                      
                  //}


              var request = {
                  PetSizeId: ctrl.selectedPetSize.PetSizeId,
                  PetCategoryId: ctrl.selectedPetCategory.PetCategoryId,
                  PetCareOptionId: ctrl.selectedPetCareOption.PetCareOptionId,
                  Zip: ctrl.zip,
                  City: ctrl.city,
                  State:ctrl.state,
                  DropOffDate: ctrl.fromDt,
                  PickUpDate:ctrl.toDt
              };


              $http.post('/home/FindPetSitters', request)
                .then(function (response) {
                    ctrl.petsitters = response.data.Result;
                    if (ctrl.petsitters.length > 0) {
                        ctrl.resultFound = true;
                    }
                  }).catch(function () {
                      alert("no result found!");
                  }
                  );

          }

      }

      //Search bar after the form is submitted


  });