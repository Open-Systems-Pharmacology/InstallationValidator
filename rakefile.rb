require_relative 'scripts/setup'
require_relative 'scripts/copy-dependencies'
require_relative 'scripts/utils'


task :create_setup, [:product_version, :configuration] do |t, args|

	setup_dir = File.join(solution_dir, 'setup')
	src_dir = src_dir_for(args.configuration)
	relative_src_dir = relative_src_dir_for(args.configuration)
	product_version = args.product_version
	suite_name = 'Open Systems Pharmacology Suite'

	#Ignore files from automatic harvesting that will be installed specifically
	harvest_ignored_files = [
		'InstallationValidator.exe' 
	]

	#Files required for setup creation only and that will not be harvested automatically
	setup_files	 = [
		"#{relative_src_dir}/TeXTemplates/**/*.*",
		'data/*.wxs',
		'src/InstallationValidator/*.ico',
		'dimensions/*.xml',
		'setup/setup.wxs',
		'setup/**/*.{msm,rtf,bmp}',
	]

	Rake::Task['setup:create'].execute(OpenStruct.new(
		solution_dir: solution_dir,
		src_dir: src_dir, 
		setup_dir: setup_dir,  
		product_name: product_name, 
		product_version: product_version,
		harvest_ignored_files: harvest_ignored_files,		
		suite_name: suite_name,
		setup_files: setup_files,
		manufacturer: manufacturer
		))
end

task :postclean do |t, args| 
	src_dir =  src_dir_for("Debug")

	all_users_dir = ENV['ALLUSERSPROFILE']
	all_users_application_dir = File.join(all_users_dir, manufacturer, product_name, '11.0')

	copy_dependencies solution_dir,  all_users_application_dir do
		copy_dimensions_xml
	end

	copy_dependencies src_dir,  File.join(all_users_application_dir, 'TeXTemplates', 'StandardTemplate') do
		copy_files 'StandardTemplate', '*'
	end
end

private

def relative_src_dir_for(configuration)
	File.join('src', 'InstallationValidator', 'bin', configuration, 'net472')
end

def src_dir_for(configuration)
	File.join(solution_dir, relative_src_dir_for(configuration))
end


def solution_dir
	File.dirname(__FILE__)
end

def	manufacturer
	'Open Systems Pharmacology'
end

def	product_name
	'InstallationValidator'
end
