$(document).ready(function () {
    function calculateFinalPayment() {
        var hoursWorked = parseFloat($('#HoursWorked').val());
        var hourlyRate = parseFloat($('#HourlyRate').val());
        var finalPayment = 0;

        if (!isNaN(hoursWorked) && !isNaN(hourlyRate)) {
            finalPayment = hoursWorked * hourlyRate;
            // Format the final payment to two decimal places
            finalPayment = finalPayment.toFixed(2);
        }

        $('#FinalPayment').val(finalPayment);
    }

    function validateInput() {
        var isValid = true;
        var hoursWorked = $('#HoursWorked').val();
        var hourlyRate = $('#HourlyRate').val();

        // Reset validation messages
        $('#HoursWorked').removeClass('is-invalid');
        $('#HourlyRate').removeClass('is-invalid');

        if (hoursWorked === '' || isNaN(hoursWorked) || parseFloat(hoursWorked) <= 0) {
            $('#HoursWorked').addClass('is-invalid');
            isValid = false;
        }

        if (hourlyRate === '' || isNaN(hourlyRate) || parseFloat(hourlyRate) <= 0) {
            $('#HourlyRate').addClass('is-invalid');
            isValid = false;
        }

        return isValid;
    }

    // Attach event handlers to input fields
    $('#HoursWorked, #HourlyRate').on('input', function () {
        calculateFinalPayment();
        validateInput();
    });

    // Form submission validation
    $('form').on('submit', function (e) {
        if (!validateInput()) {
            e.preventDefault(); // Prevent form submission
            alert('Please correct the errors in the form.');
        }
    });

    // Initial calculation and validation
    calculateFinalPayment();
    validateInput();
});
