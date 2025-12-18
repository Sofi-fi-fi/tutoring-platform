using AutoMapper;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Services.DTOs.City;
using TutoringPlatform.Services.DTOs.Subject;
using TutoringPlatform.Services.DTOs.TeachingLevel;
using TutoringPlatform.Services.DTOs.User;
using TutoringPlatform.Services.DTOs.Student;
using TutoringPlatform.Services.DTOs.Tutor;
using TutoringPlatform.Services.DTOs.TutorSubject;
using TutoringPlatform.Services.DTOs.Schedule;
using TutoringPlatform.Services.DTOs.Booking;
using TutoringPlatform.Services.DTOs.Review;

namespace TutoringPlatform.Services.Mapping;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
        CreateMap<City, CityDto>();
        CreateMap<CreateCityDto, City>()
			.ForMember(dest => dest.CityId, opt => opt.Ignore())
			.ForMember(dest => dest.Students, opt => opt.Ignore())
			.ForMember(dest => dest.Tutors, opt => opt.Ignore());
        CreateMap<UpdateCityDto, City>()
			.ForMember(dest => dest.CityId, opt => opt.Ignore())
			.ForMember(dest => dest.Students, opt => opt.Ignore())
			.ForMember(dest => dest.Tutors, opt => opt.Ignore());

        CreateMap<Subject, SubjectDto>();
        CreateMap<CreateSubjectDto, Subject>()
			.ForMember(dest => dest.SubjectId, opt => opt.Ignore())
			.ForMember(dest => dest.TutorSubjects, opt => opt.Ignore());
        CreateMap<UpdateSubjectDto, Subject>()
			.ForMember(dest => dest.SubjectId, opt => opt.Ignore())
			.ForMember(dest => dest.TutorSubjects, opt => opt.Ignore());

        CreateMap<TeachingLevel, TeachingLevelDto>();
        CreateMap<CreateTeachingLevelDto, TeachingLevel>()
			.ForMember(dest => dest.LevelId, opt => opt.Ignore())
			.ForMember(dest => dest.TutorSubjects, opt => opt.Ignore());
        CreateMap<UpdateTeachingLevelDto, TeachingLevel>()
			.ForMember(dest => dest.LevelId, opt => opt.Ignore())
			.ForMember(dest => dest.TutorSubjects, opt => opt.Ignore());

		CreateMap<User, UserDto>();
		CreateMap<CreateUserDto, User>()
			.ForMember(dest => dest.UserId, opt => opt.Ignore())
			.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
			.ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
			.ForMember(dest => dest.Student, opt => opt.Ignore())
			.ForMember(dest => dest.Tutor, opt => opt.Ignore());
		CreateMap<UpdateUserDto, User>()
			.ForMember(dest => dest.UserId, opt => opt.Ignore())
			.ForMember(dest => dest.UserType, opt => opt.Ignore())
			.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
			.ForMember(dest => dest.DateOfBirth, opt => opt.Ignore())
			.ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
			.ForMember(dest => dest.Student, opt => opt.Ignore())
			.ForMember(dest => dest.Tutor, opt => opt.Ignore());

		CreateMap<Student, StudentDto>();
		CreateMap<CreateStudentDto, Student>()
			.ForMember(dest => dest.StudentId, opt => opt.Ignore())
			.ForMember(dest => dest.City, opt => opt.Ignore())
			.ForMember(dest => dest.Bookings, opt => opt.Ignore());
		CreateMap<UpdateStudentDto, Student>()
			.ForMember(dest => dest.StudentId, opt => opt.Ignore())
			.ForMember(dest => dest.User, opt => opt.Ignore())
			.ForMember(dest => dest.City, opt => opt.Ignore())
			.ForMember(dest => dest.Bookings, opt => opt.Ignore());
		
		CreateMap<Tutor, TutorDto>();
		CreateMap<CreateTutorDto, Tutor>()
			.ForMember(dest => dest.TutorId, opt => opt.Ignore())
			.ForMember(dest => dest.City, opt => opt.Ignore())
			.ForMember(dest => dest.TutorSubjects, opt => opt.Ignore())
			.ForMember(dest => dest.Schedules, opt => opt.Ignore());
		CreateMap<UpdateTutorDto, Tutor>()
			.ForMember(dest => dest.TutorId, opt => opt.Ignore())
			.ForMember(dest => dest.User, opt => opt.Ignore())
			.ForMember(dest => dest.City, opt => opt.Ignore())
			.ForMember(dest => dest.TutorSubjects, opt => opt.Ignore())
			.ForMember(dest => dest.Schedules, opt => opt.Ignore());

		CreateMap<TutorSubject, TutorSubjectDto>();
		CreateMap<CreateTutorSubjectDto, TutorSubject>()
			.ForMember(dest => dest.TutorSubjectId, opt => opt.Ignore())
			.ForMember(dest => dest.Tutor, opt => opt.Ignore())
			.ForMember(dest => dest.Subject, opt => opt.Ignore())
			.ForMember(dest => dest.TeachingLevel, opt => opt.Ignore())
			.ForMember(dest => dest.Bookings, opt => opt.Ignore());
		CreateMap<UpdateTutorSubjectDto, TutorSubject>()
			.ForMember(dest => dest.TutorSubjectId, opt => opt.Ignore())
			.ForMember(dest => dest.TutorId, opt => opt.Ignore())
			.ForMember(dest => dest.SubjectId, opt => opt.Ignore())
			.ForMember(dest => dest.LevelId, opt => opt.Ignore())
			.ForMember(dest => dest.Tutor, opt => opt.Ignore())
			.ForMember(dest => dest.Subject, opt => opt.Ignore())
			.ForMember(dest => dest.TeachingLevel, opt => opt.Ignore())
			.ForMember(dest => dest.Bookings, opt => opt.Ignore());

		CreateMap<Schedule, ScheduleDto>();
		CreateMap<CreateScheduleDto, Schedule>()
			.ForMember(dest => dest.ScheduleId, opt => opt.Ignore())
			.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.Tutor, opt => opt.Ignore())
			.ForMember(dest => dest.Booking, opt => opt.Ignore());
		CreateMap<UpdateScheduleDto, Schedule>()
			.ForMember(dest => dest.ScheduleId, opt => opt.Ignore())
			.ForMember(dest => dest.TutorId, opt => opt.Ignore())
			.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.Tutor, opt => opt.Ignore())
			.ForMember(dest => dest.Booking, opt => opt.Ignore());

		CreateMap<Booking, BookingDto>();
		CreateMap<CreateBookingDto, Booking>()
			.ForMember(dest => dest.BookingId, opt => opt.Ignore())
			.ForMember(dest => dest.Status, opt => opt.Ignore())
			.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.Student, opt => opt.Ignore())
			.ForMember(dest => dest.TutorSubject, opt => opt.Ignore())
			.ForMember(dest => dest.Schedule, opt => opt.Ignore())
			.ForMember(dest => dest.Review, opt => opt.Ignore());
		CreateMap<UpdateBookingDto, Booking>()
			.ForMember(dest => dest.BookingId, opt => opt.Ignore())
			.ForMember(dest => dest.StudentId, opt => opt.Ignore())
			.ForMember(dest => dest.TutorSubjectId, opt => opt.Ignore())
			.ForMember(dest => dest.ScheduleId, opt => opt.Ignore())
			.ForMember(dest => dest.Format, opt => opt.Ignore())
			.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.Student, opt => opt.Ignore())
			.ForMember(dest => dest.TutorSubject, opt => opt.Ignore())
			.ForMember(dest => dest.Schedule, opt => opt.Ignore())
			.ForMember(dest => dest.Review, opt => opt.Ignore());

		CreateMap<Review, ReviewDto>();
		CreateMap<CreateReviewDto, Review>()
			.ForMember(dest => dest.ReviewId, opt => opt.Ignore())
			.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.Booking, opt => opt.Ignore());
		CreateMap<UpdateReviewDto, Review>()
			.ForMember(dest => dest.ReviewId, opt => opt.Ignore())
			.ForMember(dest => dest.BookingId, opt => opt.Ignore())
			.ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
			.ForMember(dest => dest.Booking, opt => opt.Ignore());

        CreateMap<(int TutorId, string TutorName, decimal MinRate, decimal MaxRate, decimal AvgRate), TutorPricingStatisticsDto>()
            .ForMember(dest => dest.TutorId, opt => opt.MapFrom(src => src.TutorId))
            .ForMember(dest => dest.TutorName, opt => opt.MapFrom(src => src.TutorName))
            .ForMember(dest => dest.MinRate, opt => opt.MapFrom(src => src.MinRate))
            .ForMember(dest => dest.MaxRate, opt => opt.MapFrom(src => src.MaxRate))
            .ForMember(dest => dest.AvgRate, opt => opt.MapFrom(src => src.AvgRate));

		CreateMap<(Tutor Tutor, double AverageRating, int ReviewCount), TutorTopRatedDto>()
			.ForMember(dest => dest.Tutor, opt => opt.MapFrom(src => src.Tutor))
			.ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.AverageRating))
			.ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.ReviewCount));
	}
}