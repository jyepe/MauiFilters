namespace MauiFilters.ModelView
{
    public partial class MauiFilterViewModel : ObservableObject
    {
        [ObservableProperty]
        private ImageSource _imageSource;

        public MauiFilterViewModel()
        {
            ImageSource = ImageSource.FromFile("placeholder.jpg");
        }

        [RelayCommand]
        private async Task TakePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage
                    var localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    await using var sourceStream = await photo.OpenReadAsync();
                    await using var localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);
                    ImageSource = ImageSource.FromFile(localFilePath);
                }
            }
        }
    }
}
