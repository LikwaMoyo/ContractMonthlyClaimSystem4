document.addEventListener('DOMContentLoaded', function () {
    const fileInput = document.querySelector('input[name="uploadedFiles"]');
    const allowedExtensions = ['.pdf', '.docx', '.xlsx'];
    const maxFileSize = 10 * 1024 * 1024; // 10 MB

    if (fileInput) {
        fileInput.addEventListener('change', function () {
            const files = fileInput.files;
            let isValid = true;
            let errorMessage = '';

            for (let i = 0; i < files.length; i++) {
                const file = files[i];
                const fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();

                // Check file size
                if (file.size > maxFileSize) {
                    isValid = false;
                    errorMessage = `File "${file.name}" exceeds the maximum allowed size of 10 MB.`;
                    break;
                }

                // Check file extension
                if (!allowedExtensions.includes(fileExtension)) {
                    isValid = false;
                    errorMessage = `File "${file.name}" has an invalid file type. Allowed types are: ${allowedExtensions.join(', ')}`;
                    break;
                }
            }

            if (!isValid) {
                alert(errorMessage);
                fileInput.value = ''; // Reset the input
            }
        });
    }
});
